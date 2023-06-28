using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerDialogMessagesWPF
{
    public sealed class MessengerDialogMessagesWPFInfo
    {
        private MessengerDialogWPF m_MessengerDialog;
        private Action<byte[]> m_ShowImage;
        private Action<bool> m_SetStateMessageToReaded;
        private Func<string, byte[]> m_GetBytesForFileType;
        private Dictionary<string, string> m_HyperLinksDict;
        private double m_FactWidth;
        private double m_FactHeight;
        private int m_MaxIndexIncomeSP;
        private int m_MaxIndexOutgoingSP;
        private bool m_MessageHasAttachment;
        //--------------------------------------------------------------
        public MessengerDialogMessagesWPFInfo(Func<string, byte[]> _GetBytesForFileType, Action<byte[]> _ShowImage,
            MessengerDialogWPF _MessengerDialog, Action<bool> _SetStateMessageToReaded, double _FactWidth, double _FactHeight)
        {
            m_HyperLinksDict = new Dictionary<string, string>();
            m_GetBytesForFileType = _GetBytesForFileType;
            m_ShowImage = _ShowImage;
            m_MessengerDialog = _MessengerDialog;
            m_SetStateMessageToReaded = _SetStateMessageToReaded;
            m_FactWidth = _FactWidth;
            m_FactHeight = _FactHeight;
            m_MaxIndexIncomeSP = 0;
            m_MaxIndexOutgoingSP = 0;
        }
        //--------------------------------------------------------------
        public MessengerDialogWPF MessengerDialog
        {
            get
            {
                return m_MessengerDialog;
            }
        }
        //--------------------------------------------------------------
        public Dictionary<string, string> HyperLinksDict
        {
            get
            {
                return m_HyperLinksDict;
            }
        }
        //--------------------------------------------------------------
        public bool MessageHasAttachment
        {
            get
            {
                return m_MessageHasAttachment;
            }

            set
            {
                m_MessageHasAttachment = value;
            }
        }
        //--------------------------------------------------------------
        public double FactHeight
        {
            get
            {
                return m_FactHeight;
            }
        }
        //--------------------------------------------------------------
        public double FactWidth
        {
            get
            {
                return m_FactWidth;
            }
        }
        //--------------------------------------------------------------
        public Func<string, byte[]> GetBytesForFileType
        {
            get
            {
                return m_GetBytesForFileType;
            }
        }
        //--------------------------------------------------------------
        public Action<byte[]> ShowImage
        {
            get
            {
                return m_ShowImage;
            }
        }
        //--------------------------------------------------------------
        public Action<bool> SetStateMessageToReaded
        {
            get
            {
                return m_SetStateMessageToReaded;
            }
        }
        //--------------------------------------------------------------
        public int MaxIndexIncomeSP
        {
            get
            {
                return m_MaxIndexIncomeSP;
            }

            set
            {
                m_MaxIndexIncomeSP = value;
            }
        }
        //--------------------------------------------------------------
        public int MaxIndexOutgoingSP
        {
            get
            {
                return m_MaxIndexOutgoingSP;
            }

            set
            {
                m_MaxIndexOutgoingSP = value;
            }
        }
        //--------------------------------------------------------------
    }
}
