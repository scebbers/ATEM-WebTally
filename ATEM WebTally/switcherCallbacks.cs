using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDSwitcherAPI;

namespace ATEM_WebTally
{
    public delegate void SwitcherEventHandler(object sender, object args);

    class switcherCallback : IBMDSwitcherCallback
    {
        // The only event on the main switcher:

        public event SwitcherEventHandler atemDisconnected;
        
        void IBMDSwitcherCallback.Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            if (eventType == _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected)
            {
                if (atemDisconnected != null)
                    atemDisconnected(this, null);
            }
        }

        public switcherCallback()
        {
        }
    }

    class inputCallback : IBMDSwitcherInputCallback
    {
        // Events:

        public event SwitcherEventHandler previewTallyChanged;
        public event SwitcherEventHandler programTallyChanged;
        public event SwitcherEventHandler shortNameChanged;
        public event SwitcherEventHandler longNameChanged;

        private IBMDSwitcherInput m_input;
        public IBMDSwitcherInput Input { get { return m_input; } }

        public inputCallback(IBMDSwitcherInput input)
        {
            m_input = input;
        }

        void IBMDSwitcherInputCallback.Notify(_BMDSwitcherInputEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeIsPreviewTalliedChanged:
                    if (previewTallyChanged != null)
                        previewTallyChanged(this, null);
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeIsProgramTalliedChanged:
                    if (programTallyChanged != null)
                        programTallyChanged(this, null);
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeShortNameChanged:
                    if (shortNameChanged != null)
                        shortNameChanged(this, null);
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeLongNameChanged:
                    if (longNameChanged != null)
                        longNameChanged(this, null);
                    break;
            }
        }
    }
}
