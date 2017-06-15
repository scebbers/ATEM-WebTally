using System.Windows.Forms;

namespace ATEM_WebTally
{
    partial class WebTallyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebTallyForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.consoleBox = new System.Windows.Forms.TextBox();
            this.Instellingen = new System.Windows.Forms.GroupBox();
            this.btnRefreshIpList = new System.Windows.Forms.Button();
            this.btnQR = new System.Windows.Forms.Button();
            this.autoReconnect = new System.Windows.Forms.CheckBox();
            this.btnConnectAtem = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.inputList = new System.Windows.Forms.CheckedListBox();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ipList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.serverIPbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ipInput = new System.Windows.Forms.TextBox();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.ipInputLabel = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOver = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.Instellingen.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 247F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.consoleBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Instellingen, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(735, 411);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // consoleBox
            // 
            this.consoleBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.consoleBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleBox.Location = new System.Drawing.Point(262, 15);
            this.consoleBox.Margin = new System.Windows.Forms.Padding(15);
            this.consoleBox.Multiline = true;
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.ReadOnly = true;
            this.consoleBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleBox.Size = new System.Drawing.Size(458, 381);
            this.consoleBox.TabIndex = 5;
            // 
            // Instellingen
            // 
            this.Instellingen.Controls.Add(this.btnRefreshIpList);
            this.Instellingen.Controls.Add(this.btnQR);
            this.Instellingen.Controls.Add(this.autoReconnect);
            this.Instellingen.Controls.Add(this.btnConnectAtem);
            this.Instellingen.Controls.Add(this.label4);
            this.Instellingen.Controls.Add(this.inputList);
            this.Instellingen.Controls.Add(this.lblSpeed);
            this.Instellingen.Controls.Add(this.label3);
            this.Instellingen.Controls.Add(this.ipList);
            this.Instellingen.Controls.Add(this.label2);
            this.Instellingen.Controls.Add(this.serverIPbox);
            this.Instellingen.Controls.Add(this.label1);
            this.Instellingen.Controls.Add(this.ipInput);
            this.Instellingen.Controls.Add(this.btnStopServer);
            this.Instellingen.Controls.Add(this.ipInputLabel);
            this.Instellingen.Controls.Add(this.btnStartServer);
            this.Instellingen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Instellingen.Location = new System.Drawing.Point(3, 3);
            this.Instellingen.Name = "Instellingen";
            this.Instellingen.Size = new System.Drawing.Size(241, 405);
            this.Instellingen.TabIndex = 8;
            this.Instellingen.TabStop = false;
            this.Instellingen.Text = "Instellingen";
            // 
            // btnRefreshIpList
            // 
            this.btnRefreshIpList.Location = new System.Drawing.Point(157, 251);
            this.btnRefreshIpList.Name = "btnRefreshIpList";
            this.btnRefreshIpList.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshIpList.TabIndex = 19;
            this.btnRefreshIpList.Text = "Ververs lijst";
            this.btnRefreshIpList.UseVisualStyleBackColor = true;
            this.btnRefreshIpList.Click += new System.EventHandler(this.btnRefreshIpList_Click);
            // 
            // btnQR
            // 
            this.btnQR.Enabled = false;
            this.btnQR.Location = new System.Drawing.Point(157, 337);
            this.btnQR.Name = "btnQR";
            this.btnQR.Size = new System.Drawing.Size(75, 23);
            this.btnQR.TabIndex = 18;
            this.btnQR.Text = "QR";
            this.btnQR.UseVisualStyleBackColor = true;
            this.btnQR.Click += new System.EventHandler(this.btnQR_Click);
            // 
            // autoReconnect
            // 
            this.autoReconnect.AutoSize = true;
            this.autoReconnect.Location = new System.Drawing.Point(9, 55);
            this.autoReconnect.Name = "autoReconnect";
            this.autoReconnect.Size = new System.Drawing.Size(99, 17);
            this.autoReconnect.TabIndex = 17;
            this.autoReconnect.Text = "Auto-reconnect";
            this.autoReconnect.UseVisualStyleBackColor = true;
            // 
            // btnConnectAtem
            // 
            this.btnConnectAtem.Location = new System.Drawing.Point(157, 51);
            this.btnConnectAtem.Name = "btnConnectAtem";
            this.btnConnectAtem.Size = new System.Drawing.Size(75, 23);
            this.btnConnectAtem.TabIndex = 16;
            this.btnConnectAtem.Text = "Verbinden";
            this.btnConnectAtem.UseVisualStyleBackColor = true;
            this.btnConnectAtem.Click += new System.EventHandler(this.btnConnectAtem_click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Gebruikte inputs:";
            // 
            // inputList
            // 
            this.inputList.CheckOnClick = true;
            this.inputList.FormattingEnabled = true;
            this.inputList.Location = new System.Drawing.Point(8, 105);
            this.inputList.Name = "inputList";
            this.inputList.Size = new System.Drawing.Size(224, 94);
            this.inputList.TabIndex = 14;
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(67, 375);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(0, 13);
            this.lblSpeed.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 375);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Requests:";
            // 
            // ipList
            // 
            this.ipList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ipList.FormattingEnabled = true;
            this.ipList.Location = new System.Drawing.Point(9, 224);
            this.ipList.Name = "ipList";
            this.ipList.Size = new System.Drawing.Size(222, 21);
            this.ipList.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "IP-adres server:";
            // 
            // serverIPbox
            // 
            this.serverIPbox.Location = new System.Drawing.Point(9, 339);
            this.serverIPbox.Name = "serverIPbox";
            this.serverIPbox.ReadOnly = true;
            this.serverIPbox.Size = new System.Drawing.Size(122, 20);
            this.serverIPbox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 323);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Server adres:";
            // 
            // ipInput
            // 
            this.ipInput.Location = new System.Drawing.Point(103, 24);
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(129, 20);
            this.ipInput.TabIndex = 1;
            this.ipInput.Text = global::ATEM_WebTally.Properties.Settings.Default.stdIP;
            // 
            // btnStopServer
            // 
            this.btnStopServer.Enabled = false;
            this.btnStopServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopServer.Location = new System.Drawing.Point(133, 289);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(99, 23);
            this.btnStopServer.TabIndex = 4;
            this.btnStopServer.Text = "Stop server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_click);
            // 
            // ipInputLabel
            // 
            this.ipInputLabel.AutoSize = true;
            this.ipInputLabel.Location = new System.Drawing.Point(6, 27);
            this.ipInputLabel.Name = "ipInputLabel";
            this.ipInputLabel.Size = new System.Drawing.Size(91, 13);
            this.ipInputLabel.TabIndex = 0;
            this.ipInputLabel.Text = "IP-adres switcher:";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Enabled = false;
            this.btnStartServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartServer.Location = new System.Drawing.Point(9, 289);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(99, 23);
            this.btnStartServer.TabIndex = 3;
            this.btnStartServer.Text = "Start server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(735, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnHelp,
            this.btnOver});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // btnHelp
            // 
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(99, 22);
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnOver
            // 
            this.btnOver.Name = "btnOver";
            this.btnOver.Size = new System.Drawing.Size(99, 22);
            this.btnOver.Text = "Over";
            this.btnOver.Click += new System.EventHandler(this.btnOver_Click);
            // 
            // WebTallyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 435);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(735, 441);
            this.Name = "WebTallyForm";
            this.Text = "ATEM WebTally";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.Instellingen.ResumeLayout(false);
            this.Instellingen.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox Instellingen;
        private TextBox ipInput;
        private Label ipInputLabel;
        private Label label1;
        private ComboBox ipList;
        private Label label2;
        private Label lblSpeed;
        private Label label3;
        public TextBox consoleBox;
        private Button btnConnectAtem;
        private Label label4;
        public Button btnStopServer;
        public Button btnStartServer;
        public TextBox serverIPbox;
        public CheckedListBox inputList;
        private CheckBox autoReconnect;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem btnHelp;
        private ToolStripMenuItem btnOver;
        public Button btnQR;
        private Button btnRefreshIpList;
    }
}

