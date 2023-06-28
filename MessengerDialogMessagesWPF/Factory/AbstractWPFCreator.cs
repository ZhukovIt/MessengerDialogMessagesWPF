using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MessengerDialogMessagesWPF.Factory
{
    public struct RequestInfo
    {
        private uint m_ElementType;
        private object m_lParam;
        private object m_wParam;
        //--------------------------------------------------------------
        public uint ElementType
        {
            get
            {
                return m_ElementType;
            }
        }
        //--------------------------------------------------------------
        public object lParam
        {
            get
            {
                return m_lParam;
            }
        }
        //--------------------------------------------------------------
        public object wParam
        {
            get
            {
                return m_wParam;
            }
        }
        //--------------------------------------------------------------
        public RequestInfo(uint _ElementType)
        {
            m_ElementType = _ElementType;
            m_lParam = new object();
            m_wParam = new object();
        }
        //--------------------------------------------------------------
        public RequestInfo(uint _ElementType, object _lParam)
        {
            m_ElementType = _ElementType;
            m_lParam = _lParam;
            m_wParam = new object();
        }
        //--------------------------------------------------------------
        public RequestInfo(uint _ElementType, object _lParam, object _wParam)
        {
            m_ElementType = _ElementType;
            m_lParam = _lParam;
            m_wParam = _wParam;
        }
        //--------------------------------------------------------------
    }
    //--------------------------------------------------------------
    public abstract class AbstractWPFCreator
    {
        protected ResourceDictionary Resources { get; set; }
        //--------------------------------------------------------------
        public AbstractWPFCreator(ResourceDictionary _Resources)
        {
            Resources = _Resources;
        }
        //--------------------------------------------------------------
        public abstract FrameworkElement Create(RequestInfo _RequestInfo);
        //--------------------------------------------------------------
    }
}
