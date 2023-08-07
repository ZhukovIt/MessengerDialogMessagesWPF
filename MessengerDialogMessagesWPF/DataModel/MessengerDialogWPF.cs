using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerDialogMessagesWPF
{
    public sealed class MessengerDialogWPF
    {
        private bool m_IsRead;
        private Action<MessengerDialogWPF> m_ActionDelegateIsReadChanged;
        private event Action<MessengerDialogWPF> m_ChangedIsRead;
        //--------------------------------------------------------------
        public int Id { get; set; }
        //--------------------------------------------------------------
        public List<MessengerDialogMessage> Messages { get; set; }
        //--------------------------------------------------------------
        public bool IsRead
        {
            get
            {
                return m_IsRead;
            }

            set
            {
                m_IsRead = value;
                m_ChangedIsRead?.Invoke(this);
            }
        }
        //--------------------------------------------------------------
        public string ProfileURL { get; set; }
        //--------------------------------------------------------------
        public bool IsComment { get; set; }
        //--------------------------------------------------------------
        public void SetIsReadChangedEventHandler(Action<MessengerDialogWPF> _Handler)
        {
            if (m_ActionDelegateIsReadChanged == null)
            {
                m_ActionDelegateIsReadChanged = _Handler;

                m_ChangedIsRead += _Handler;
            }
        }
        //--------------------------------------------------------------
    }
}
