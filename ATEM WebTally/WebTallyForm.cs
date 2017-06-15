using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Web.Script.Serialization;

// Needed for ATEM Switcher API
using System.Runtime.InteropServices;
using BMDSwitcherAPI;

// Library for generating QR codes
using QRCoder;

namespace ATEM_WebTally
{
    public partial class WebTallyForm : Form
    {
        public class AtemConnection
        {
            // Class for handling the connection to ATEM. Initializes, creates callbacklisteners en updates the
            // inputInfo list.
            //
            // m_switcher is the instance used for listening on changes. Gets populated after ConnectTo-call on
            // m_switcherDiscovery
            public static IBMDSwitcher m_switcher;
            private WebTallyForm parent;

            private switcherCallback m_switcherCallback;
            private List<inputCallback> m_inputCallbacks = new List<inputCallback>();

            public List<Dictionary<string, string>> inputInfo = new List<Dictionary<string, string>>();

            public AtemConnection(WebTallyForm p)
            {
                // Upon creation define the parent object
                this.parent = p;
                Start();
            }

            private void Start()
            {
                
                isListeningOnAtem = false;
                
                _BMDSwitcherConnectToFailure failReason = 0;
                string address = parent.switcherIP;

                parent.btnConnectAtem.Enabled = false;

                Log("Verbinding maken met Atem Switcher op " + address);

                try
                {
                    // Note that ConnectTo() can take several seconds to return, both for success or failure,
                    // depending upon hostname resolution and network response times. Unfortunately, BMD Switcher
                    // API does not support cross thread calls, so running it in a seperate thread will result
                    // in an error.

                    m_switcherDiscovery.ConnectTo(address, out m_switcher, out failReason);
                }
                catch (Exception e)
                {
                    parent.btnConnectAtem.Enabled = true;

                    // An exception will be thrown if ConnectTo fails. For more information, see failReason.
                    switch (failReason)
                    {
                        case _BMDSwitcherConnectToFailure.bmdSwitcherConnectToFailureNoResponse:
                            Log("Geen antwoord van de Atem Switcher");
                            break;
                        case _BMDSwitcherConnectToFailure.bmdSwitcherConnectToFailureIncompatibleFirmware:
                            Log("De firmwareversie komt niet overeen");
                            break;
                        default:
                            Log("Connectie niet mogelijk vanwege onbekende fout");
                            break;
                    }
                    Log(e.Message);
                    Task.Run(() =>
                    {
                        // If connection fails, try again after 5 seconds, if the checkbox is checked.

                        if (parent.autoReconnect.Checked)
                        {
                            Log("Over 5 seconden weer proberen.");
                            Thread.Sleep(5000);
                            parent.Invoke((Action)(() => {
                                AtemInfo = new AtemConnection(parent);
                            }));
                        }
                        
                    });

                    return;
                }

                // Create the callback for listening for loss of connection.
                // The callbacks and event types are defined in switcherCallbacks.cs. 
                // We make use of the the Lambda notation, calling Invoke on the parent object,
                // because we want the action (atemDisconnected) to be performed on de parent
                // thread.

                m_switcherCallback = new switcherCallback();
                m_switcherCallback.atemDisconnected += new SwitcherEventHandler((s, a) => this.parent.Invoke((Action)(() => atemDisconnected())));

                m_switcher.AddCallback(m_switcherCallback);

                // The standard IP is defined in the settings. Update if connection was succesfull

                Log("Verbonden met Atem Switcher");
                Properties.Settings.Default.stdIP = address;
                Properties.Settings.Default.Save();
                isListeningOnAtem = true;

                parent.btnStartServer.Enabled = true;
                parent.autoReconnect.Checked = true;

                updateInputInfo(true, true);

            }

            public void updateInputInfo(bool updateCallbacks, bool updateInputList)
            {
                // Get the info on inputs, and update the inputList.

                IBMDSwitcherInputIterator inputIterator = null;
                IntPtr inputIteratorPtr;
                Guid inputIteratorIID = typeof(IBMDSwitcherInputIterator).GUID;
                m_switcher.CreateIterator(ref inputIteratorIID, out inputIteratorPtr);
                if (inputIteratorPtr != null)
                {
                    inputIterator = (IBMDSwitcherInputIterator)Marshal.GetObjectForIUnknown(inputIteratorPtr);
                }

                if (inputIterator == null)
                    return;

                IBMDSwitcherInput input;
                inputIterator.Next(out input);

                List<int> ci = new List<int>();

                // Ports are assigned for each input, so that a HttpListener intance can be created for listening
                // on each port separately. Starting port is 10000, but can be any port.

                int tallyPort = 10000;
                if (updateInputList)
                {
                    // Remember the currently checked inputs.
                    ci = parent.inputList.CheckedIndices.Cast<int>().ToList<int>();
                    parent.inputList.Items.Clear();
                }

                inputPorts.Clear();

                List<Dictionary<string, string>> inputInfoTemp = new List<Dictionary<string, string>>();

                while (input != null)
                {
                    if (updateCallbacks)
                    {
                        // Create the callback for listening for input name changes and tally changes.
                        // The callbacks and event types are defined in switcherCallbacks.cs. 
                        // We make use of the the Lambda notation, calling Invoke on the parent object,
                        // because we want the action (updateInputInfo) to be performed on de parent
                        // thread.

                        inputCallback newInputMonitor = new inputCallback(input);
                        input.AddCallback(newInputMonitor);
                        newInputMonitor.previewTallyChanged += new SwitcherEventHandler((a, b) => this.parent.Invoke((Action)(() => updateInputInfo(false, false))));
                        newInputMonitor.programTallyChanged += new SwitcherEventHandler((a, b) => this.parent.Invoke((Action)(() => updateInputInfo(false, false))));
                        newInputMonitor.shortNameChanged += new SwitcherEventHandler((a, b) => this.parent.Invoke((Action)(() => updateInputInfo(false, true))));
                        newInputMonitor.longNameChanged += new SwitcherEventHandler((a, b) => this.parent.Invoke((Action)(() => updateInputInfo(false, true))));

                        m_inputCallbacks.Add(newInputMonitor);
                    }

                    Dictionary<string, string> inputItem = new Dictionary<string, string>();

                    // Get the tally state and the name of the current input.

                    input.IsPreviewTallied(out int isPreview);
                    input.IsProgramTallied(out int isProgram);
                    input.GetShortName(out string shortName);
                    input.GetLongName(out string longName);

                    inputItem.Add("isPreview", isPreview.ToString());
                    inputItem.Add("isProgram", isProgram.ToString());
                    inputItem.Add("shortName", shortName);
                    inputItem.Add("longName", longName);
                    
                    inputInfoTemp.Add(inputItem);

                    if (updateInputList)
                    {
                        parent.inputList.Items.Add(string.Format("{0}: {1}", shortName, longName));
                    }

                    inputPorts.Add(tallyPort);
                    tallyPort++;

                    inputIterator.Next(out input);
                }

                lock (locker)
                {
                    inputInfo = new List<Dictionary<string, string>>();
                    inputInfo = inputInfoTemp;
                }

                if (updateInputList)
                {
                    foreach (var i in ci)
                    {
                        // When the inputList is updated, set the correct inputs checked.
                        parent.inputList.SetItemChecked(i, true);
                    }
                }
                
            }

            private void Log(string m)
            {
                parent.Log(m);
            }

            private void atemDisconnected()
            {
                Log("Verbinding met de ATEM Switcher verloren.");
                isListeningOnAtem = false;

                parent.btnConnectAtem.Enabled = true;

                parent.btnStartServer.Enabled = false;

                // Remove all input monitors, remove callbacks
                foreach (inputCallback inputC in m_inputCallbacks)
                {
                    inputC.Input.RemoveCallback(inputC);
                    inputC.previewTallyChanged -= new SwitcherEventHandler((a, b) => this.parent.Invoke((Action)(() => updateInputInfo(false, false))));
                    inputC.programTallyChanged -= new SwitcherEventHandler((a, b) => this.parent.Invoke((Action)(() => updateInputInfo(false, false))));
                    inputC.shortNameChanged -= new SwitcherEventHandler((a, b) => this.parent.Invoke((Action)(() => updateInputInfo(false, true))));
                    inputC.longNameChanged -= new SwitcherEventHandler((a, b) => this.parent.Invoke((Action)(() => updateInputInfo(false, true))));
                }
                m_inputCallbacks.Clear();
                
                if (m_switcher != null)
                {
                    // Remove callback:
                    m_switcher.RemoveCallback(m_switcherCallback);

                    // release reference:
                    m_switcher = null;
                }

                Task.Run(() =>
                {
                    if (parent.autoReconnect.Checked)
                    {
                        // If checkbox is checked, retry connecting after 5 seconds.
                        Thread.Sleep(5000);
                        parent.Invoke((Action)(() =>
                        {
                            AtemInfo = new AtemConnection(parent);
                        }));
                    }
                });
            }
            

        }

        
        public static AtemConnection AtemInfo;
        public static IBMDSwitcherDiscovery m_switcherDiscovery;

        public static bool isListeningOnAtem = false;
        private string switcherIP;
        public static string serverIP;

        public static List<int> inputPorts = new List<int>();

        private ReaderWriterLock rwl = new ReaderWriterLock();
        public static object locker = new object();

        public static QRForm qrf;

        private socketListener mainServer;
        private List<socketListener> clientServers = new List<socketListener>();

        public static int totalRequests = 0;

        List<Tuple<string, string>> ips;

        public WebTallyForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            
            Task cr = new Task(new Action(countRequests));
            cr.Start();

            Log("Atem Switcher Discovery wordt opgestart");

            // This program makes use of the native BMDSwitcherAPI.dll, which is installed with the ATEM
            // Switchers Software. Therefore using this product requires ATEM Switchers Software to be installed.

            try
            {
                m_switcherDiscovery = new CBMDSwitcherDiscovery();
            }
            catch(Exception e)
            {
                MessageBox.Show("Switcher Discovery kon niet worden geinitialiseerd.\r\nZorg dat u ATEM Switchers Software heeft geïnstalleerd op deze PC.", "Fout", MessageBoxButtons.OK);
                Environment.Exit(1);
            }

            if (m_switcherDiscovery != null)
            {
                Log("Atem Switcher Discovery draait");
            }
        }
        
        private void btnConnectAtem_click(object sender, EventArgs e)
        {
            // Start connection with ATEM, after checking ip-address.

            string ip = ipInput.Text;
            if (IPAddress.TryParse(ip, out IPAddress IPA))
            {
                switcherIP = ip;

                AtemInfo = new AtemConnection(this);
            }
            else
            {
                MessageBox.Show("Geef een geldig ip-adres op.", "Fout", MessageBoxButtons.OK);
            }
        }

        private void btnStartServer_click(object sender, EventArgs e)
        {
            if (ipList.SelectedIndex == -1)
            {
                MessageBox.Show("Selecteer een server IP-adres.", "Fout", MessageBoxButtons.OK);
                return;
            }
            if (inputList.CheckedItems.Count == 0)
            {
                MessageBox.Show("Selecteer minstens één input.", "Fout", MessageBoxButtons.OK);
                return;
            }
            if (isListeningOnAtem)
            {
                // Only when inputs and server are selected, continue starting the servers.
                // One mainserver is started, handling requests on port 80, and serving them
                // the index page where the selected inputs are listed.
                // The other servers each listen on specific ports for each input.

                serverIP = ips.ElementAt(ipList.SelectedIndex).Item2;
                
                Log("Mainserver opstarten...");
                mainServer = new socketListener(this, 80, "index");
                mainServer.start();

                inputList.Enabled = false;

                clientServers.Clear();

                foreach (var selectedInput in inputList.CheckedIndices.Cast<int>().ToList<int>())
                {
                    clientServers.Add(new socketListener(this, inputPorts.ElementAt(selectedInput), "client"));
                }
                foreach (var client in clientServers)
                {
                    Log("Inputlistener opstarten...");
                    client.start();
                }

            }
        }

        private void btnStopServer_click(object sender, EventArgs e)
        {
            // Close all servers.

            mainServer.close();
            foreach (var client in clientServers) {
                client.close();
            }
            
            Log("Servers gestopt");
            serverIPbox.Text = "";

            inputList.Enabled = true;

            btnStartServer.Enabled = true;
            btnStopServer.Enabled = false;
            btnQR.Enabled = false;
        }

        private List<Tuple<string, string>> updateIpList()
        {
            List<Tuple<string, string>> ipList = new List<Tuple<string, string>>();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var ua in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipList.Add(Tuple.Create(ni.Name, ua.Address.ToString()));
                    }
                }
            }
            return ipList;
        }

        private void countRequests()
        {
            // Each second, read the amount of requests and reset the variable to 0.

            Thread.Sleep(1000);
            int currRequests = totalRequests;
            totalRequests = 0;
            lblSpeed.Text = currRequests.ToString() + " /s";
            countRequests();
        }

        public void Log(string message)
        {
            // Log to the main console
            consoleBox.AppendText(DateTime.Now.ToShortTimeString() + " : " + message + "\r\n");
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            ips = updateIpList();
            foreach(var ip in ips)
            {
                ipList.Items.Add(ip.Item2 + " (" + ip.Item1 + ")");
            }

            qrf = new QRForm();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help helpBox = new Help();
            helpBox.ShowDialog();
        }

        private void btnOver_Click(object sender, EventArgs e)
        {
            About aboutBox = new About();
            aboutBox.ShowDialog();
        }

        private void btnQR_Click(object sender, EventArgs e)
        {
            qrf.ShowDialog();
        }

        private void btnRefreshIpList_Click(object sender, EventArgs e)
        {
            ips = updateIpList();
            ipList.Items.Clear();
            foreach (var ip in ips)
            {
                ipList.Items.Add(ip.Item2 + " (" + ip.Item1 + ")");
            }
        }
    }

    public class socketListener
    {
        // Main class handling all the requests from clients. Basically starts the HttpListeners, handles
        // requests for each port, reads the inputInfo, and returns this info to the client.

        private HttpListener listenerInstance;
        public HttpListener ListenerInstance
        {
            get { return listenerInstance; }
            set { listenerInstance = ListenerInstance;  }
        }

        private int localPort;
        private string type;

        private WebTallyForm _mainForm;

        private ReaderWriterLock rwl = new ReaderWriterLock();
        private object fileLocker = new object();

        public socketListener(WebTallyForm mainForm, int p, string t)
        {
            _mainForm = mainForm;
            type = t;

            _mainForm.Log("Server starten...");
            listenerInstance = new HttpListener();
            localPort = p;
        }

        private async void requestCallback(IAsyncResult ar)
        {
            // Function called when a request starts. Reads the requests until it has stopped, then start listening for 
            // a new request asynchronously, while the current request is being handled and responded to.

            HttpListenerContext ctx;
            try
            {
                ctx = listenerInstance.EndGetContext(ar);
                listenerInstance.BeginGetContext(requestCallback, null);
            }
            catch (Exception)
            {
                // An exception is thrown when the Close() is called on the HttpListener instance when 
                // a request is still coming through. Nothing to do here.

                return;
            }
            
            WebTallyForm.totalRequests++;

            if (WebTallyForm.isListeningOnAtem)
            {
                if (type == "index")
                {
                    //Handle requests on port for indexpage, deliver the main index page where clients can choose their input

                    string resPath = ctx.Request.Url.LocalPath;

                    if (resPath == "/")
                    {
                        var page = Application.StartupPath + "/webserver/main/index.htm";
                        string filedata;

                        rwl.AcquireReaderLock(Timeout.Infinite);
                        filedata = File.ReadAllText(page);
                        rwl.ReleaseReaderLock();

                        string shortName;
                        string longName;

                        string buttons = "";
                        foreach (var index in _mainForm.inputList.CheckedIndices.Cast<int>().ToList<int>())
                        {
                            WebTallyForm.AtemInfo.inputInfo.ElementAt(index).TryGetValue("shortName", out shortName);
                            WebTallyForm.AtemInfo.inputInfo.ElementAt(index).TryGetValue("longName", out longName);

                            buttons += string.Format("<a href=\"http://{0}:{1}\"><button>{2}: {3}</button></a><br /><br />\r\n", WebTallyForm.serverIP, WebTallyForm.inputPorts.ElementAt(index), shortName, longName);
                        }


                        byte[] output = Encoding.UTF8.GetBytes(filedata.Replace("<<<inputList>>>", buttons));

                        ctx.Response.ContentType = "text/html";

                        ctx.Response.StatusCode = 200;
                        try
                        {
                            await ctx.Response.OutputStream.WriteAsync(output, 0, output.Length);
                        }
                        catch (Exception ex)
                        {
                            _mainForm.Log(ex.Message);

                        }

                        ctx.Response.Close();
                    }
                    else
                    {
                        // Every other file is served as requested.

                        var page = Application.StartupPath + "/webserver/main" + resPath;

                        bool fileExist;
                        lock (fileLocker)
                            fileExist = File.Exists(page);
                        if (!fileExist)
                        {
                            var errorPage = Encoding.UTF8.GetBytes("<h1 style=\"color:red\">Error 404 , Bestand bestaat niet</h1><hr><a href=\".\\\">Back to Home</a>");
                            ctx.Response.ContentType = "text/html";
                            ctx.Response.StatusCode = 404;
                            try
                            {
                                await ctx.Response.OutputStream.WriteAsync(errorPage, 0, errorPage.Length);
                            }
                            catch (Exception ex)
                            {
                                _mainForm.Log(ex.Message);

                            }
                            ctx.Response.Close();
                            return;
                        }

                        byte[] filedata;

                        rwl.AcquireReaderLock(Timeout.Infinite);
                        filedata = File.ReadAllBytes(page);
                        rwl.ReleaseReaderLock();

                        FileInfo fileinfo = new FileInfo(page);
                        if (fileinfo.Extension == ".css")
                            ctx.Response.ContentType = "text/css";
                        else if (fileinfo.Extension == ".html" || fileinfo.Extension == ".htm")
                            ctx.Response.ContentType = "text/html";
                        else if (fileinfo.Extension == ".js")
                            ctx.Response.ContentType = "text/javascript";
                        else if (fileinfo.Extension == ".ico")
                            ctx.Response.ContentType = "image/x-icon";

                        ctx.Response.StatusCode = 200;
                        ctx.Response.KeepAlive = true;
                        try
                        {
                            await ctx.Response.OutputStream.WriteAsync(filedata, 0, filedata.Length);
                        }
                        catch (Exception ex)
                        {
                            _mainForm.Log(ex.Message);

                        }

                        ctx.Response.Close();
                    }
                    
                }
                else if (type == "client")
                {
                    //Handle a client request on the specified port:
                    // - if it is an AJAX request, deliver the tally status in JSON format
                    // - if it is a new request, deliver the main resources

                    string resPath = ctx.Request.Url.LocalPath;

                    if (ctx.Request.QueryString["type"] == "status")
                    {
                        //AJAX request

                        int camNum = WebTallyForm.inputPorts.IndexOf(localPort);
                        byte[] content;

                        string isPreview;
                        string isProgram;
                        string shortName;
                        string longName;
                        string status;

                        try
                        {
                            lock (WebTallyForm.locker)
                            {
                                WebTallyForm.AtemInfo.inputInfo.ElementAt(camNum).TryGetValue("isPreview", out isPreview);
                                WebTallyForm.AtemInfo.inputInfo.ElementAt(camNum).TryGetValue("isProgram", out isProgram);
                                WebTallyForm.AtemInfo.inputInfo.ElementAt(camNum).TryGetValue("shortName", out shortName);
                                WebTallyForm.AtemInfo.inputInfo.ElementAt(camNum).TryGetValue("longName", out longName);
                            }
                        }


                        catch (ArgumentOutOfRangeException)
                        {

                            content = Encoding.UTF8.GetBytes(
                                new JavaScriptSerializer().Serialize(new
                                {
                                    isInput = false,
                                    connection = true
                                })
                                );
                            ctx.Response.ContentType = "application/json";
                            ctx.Response.StatusCode = 200;
                            ctx.Response.KeepAlive = true;
                            try
                            {
                                await ctx.Response.OutputStream.WriteAsync(content, 0, content.Length);
                            }
                            catch (Exception e)
                            {
                                _mainForm.Log(e.Message);

                            }
                            ctx.Response.Close();
                            return;
                        }

                        if (isProgram == "1")
                        {
                            status = "live";
                        }
                        else if (isPreview == "2")
                        {
                            status = "preview";
                        }
                        else
                        {
                            status = "free";
                        }

                        content = Encoding.UTF8.GetBytes(
                            new JavaScriptSerializer().Serialize(new
                            {
                                isInput = true,
                                connection = true,
                                status = status,
                                shortName = shortName,
                                longName = longName
                            })
                            );

                        ctx.Response.ContentType = "application/json";
                        ctx.Response.StatusCode = 200;
                        ctx.Response.KeepAlive = true;
                        try
                        {
                            await ctx.Response.OutputStream.WriteAsync(content, 0, content.Length);
                        }
                        catch (Exception ex)
                        {
                            _mainForm.Log(ex.Message);

                        }
                        try
                        {
                            ctx.Response.Close();
                        }
                        catch (Exception e)
                        {
                            _mainForm.Log(e.Message);
                        }
                        return;
                    }
                    else
                    {
                        // Normal request

                        int camNum = WebTallyForm.inputPorts.IndexOf(localPort);
                        if (resPath == "/")
                        {
                            resPath = "/index.htm";
                            _mainForm.Log(string.Format("Client: {0}", ctx.Request.RemoteEndPoint));
                            _mainForm.Log(string.Format("--Input: {0}", camNum));
                        }

                        var page = Application.StartupPath + "/webserver/client" + resPath;

                        bool fileExist;
                        lock (fileLocker)
                            fileExist = File.Exists(page);
                        if (!fileExist)
                        {
                            var errorPage = Encoding.UTF8.GetBytes("<h1 style=\"color:red\">Error 404 , Bestand bestaat niet</h1><hr><a href=\".\\\">Back to Home</a>");
                            ctx.Response.ContentType = "text/html";
                            ctx.Response.StatusCode = 404;
                            try
                            {
                                await ctx.Response.OutputStream.WriteAsync(errorPage, 0, errorPage.Length);
                            }
                            catch (Exception ex)
                            {
                                _mainForm.Log(ex.Message);

                            }
                            ctx.Response.Close();
                            return;
                        }

                        byte[] filedata;

                        rwl.AcquireReaderLock(Timeout.Infinite);
                        filedata = File.ReadAllBytes(page);
                        rwl.ReleaseReaderLock();

                        FileInfo fileinfo = new FileInfo(page);
                        if (fileinfo.Extension == ".css")
                            ctx.Response.ContentType = "text/css";
                        else if (fileinfo.Extension == ".html" || fileinfo.Extension == ".htm")
                            ctx.Response.ContentType = "text/html";
                        else if (fileinfo.Extension == ".js")
                            ctx.Response.ContentType = "text/javascript";
                        else if (fileinfo.Extension == ".ico")
                            ctx.Response.ContentType = "image/x-icon";

                        ctx.Response.StatusCode = 200;
                        ctx.Response.KeepAlive = true;
                        try
                        {
                            await ctx.Response.OutputStream.WriteAsync(filedata, 0, filedata.Length);
                        }
                        catch (Exception ex)
                        {
                            _mainForm.Log(ex.Message);

                        }

                        ctx.Response.Close();
                    }
                }
            }
            else
            {
                byte[] content = Encoding.UTF8.GetBytes(
                    new JavaScriptSerializer().Serialize(new
                    {
                        connection = false
                    })
                    );
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 200;
                try
                {
                    await ctx.Response.OutputStream.WriteAsync(content, 0, content.Length);
                }
                catch (Exception e)
                {
                    _mainForm.Log(e.Message);

                }
                ctx.Response.Close();
                return;
            }
        }

        public async void start()
        {
            // Start listening on the port for this instance, and add a firewall exception.
            // This needs administrator rights, so this program must be run in admin mode.

            if (type == "index")
            {
                _mainForm.Log(string.Format("Server IP adres: {0}", WebTallyForm.serverIP));
                _mainForm.Log(string.Format("Toegewezen poort: {0}", localPort));

            }
            else
            {
                _mainForm.Log(string.Format("Poort toevoegen voor luisteren: {0}", localPort));
            }

            try
            {
                await AddFirewallRule(localPort);
            }
            catch (Exception e)
            {
                _mainForm.Log(e.Message);
                _mainForm.inputList.Enabled = true;
                return;
            }
            

            listenerInstance.Prefixes.Clear();
            listenerInstance.Prefixes.Add(string.Format("http://{0}:{1}/", WebTallyForm.serverIP, localPort));

            try
            {
                listenerInstance.Start();
            }
            catch (Exception ex)
            {
                _mainForm.Log(string.Format("Server niet gestart. Fout: {0}", ex.Message));
                _mainForm.Log(string.Format("--Localport: {0}", localPort));
                _mainForm.Log("Zorg dat ATEM WebTally is opgestart als Adminstrator");
                _mainForm.inputList.Enabled = true;
                return;
            }

            if (listenerInstance.IsListening)
            {
                _mainForm.btnStartServer.Enabled = false;
                _mainForm.btnStopServer.Enabled = true;
                if (type == "index")
                {
                    // If mainserver is running, provide user with the server-ip and QR-code.

                    _mainForm.Log("Server succesvol gestart");
                    _mainForm.Log(string.Format("WebTally is te bereiken op http://{0}", WebTallyForm.serverIP));
                    _mainForm.serverIPbox.Text = string.Format("{0}", WebTallyForm.serverIP);

                    WebTallyForm.qrf.qrImage.Image = createQRcode(string.Format("http://{0}", WebTallyForm.serverIP));
                    _mainForm.btnQR.Enabled = true;

                }

                listenerInstance.BeginGetContext(requestCallback, null);

            }
        }

        public void close()
        {
            listenerInstance.Close();
            listenerInstance = new HttpListener();
        }

        private Task AddFirewallRule(int port)
        {
            return Task.Run(() =>
            {
                string cmd = RunCMD("netsh advfirewall firewall add rule name=\"ATEM WebTally\" dir=in action=allow remoteip=localsubnet protocol=tcp localport=" + port);
                if (cmd.Contains("Ok."))
                {
                    _mainForm.Log(string.Format("WebTally heeft de poort {0} toegevoegd aan de firewall", port));
                }
            });

        }

        private string RunCMD(string cmd)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = "/C " + cmd;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
            string res = proc.StandardOutput.ReadToEnd();
            proc.StandardOutput.Close();

            proc.Close();
            return res;
        }

        private Bitmap createQRcode(string link)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);
            
            return qrCodeImage;
        }
    }
}
