using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MessengerDialogMessagesWPF.Service
{
    public sealed class BackgroundPanelMessagesWPFService : CommonMessengerService
    {
        private readonly BackgroundPanelMessagesWPF m_MainControl;
        //--------------------------------------------------------------
        public BackgroundPanelMessagesWPFService(BackgroundPanelMessagesWPF _MainControl)
        {
            m_MainControl = _MainControl;
        }
        //--------------------------------------------------------------
        public override void AddMessage(Panel Container, MessengerDialogMessage _Message, bool _HasAttachment)
        {
            var _NewMessage = m_MainControl.Factory
                .Create(new Factory.RequestInfo((uint)Factory.BackgroundPanelMessagesWPFElementTypes.MainMessageStackPanel, _Message));

            Container.Children.Add(_NewMessage);
        }
        //--------------------------------------------------------------
        public override void UpdateMessage(Panel _Container, MessengerDialogMessage _Message)
        {
            string _Key = $"SecondaryMessageStackPanel_{_Message.Id}";

            var _SecondaryMessageStackPanel = FindFrameworkElementFromKey(_Container, _Key) as StackPanel;

            UIElement _RemovedUIElement = null;

            foreach (UIElement _Element in _SecondaryMessageStackPanel.Children)
            {
                if (_Element is StackPanel)
                {
                    _RemovedUIElement = _Element;
                }
            }

            if (_RemovedUIElement != null)
            {
                _SecondaryMessageStackPanel.Children.Remove(_RemovedUIElement);
            }

            _SecondaryMessageStackPanel.Children.Add(m_MainControl.Factory
                .Create(new Factory.RequestInfo((uint)Factory.BackgroundPanelMessagesWPFElementTypes.MessageStatusFooterStackPanel, _Message)));
        }
        //--------------------------------------------------------------
        public bool MessengerDialogMessageIsNotExistsInContainer(Panel _Container, int _MessengerDialogMessageId)
        {
            string _Key = $"SecondaryMessageStackPanel_{_MessengerDialogMessageId}";

            return FindFrameworkElementFromKey(_Container, _Key) == null;
        }
        //--------------------------------------------------------------
    }
}
