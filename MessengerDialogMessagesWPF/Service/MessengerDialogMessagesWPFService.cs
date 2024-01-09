using MessengerDialogMessagesWPF.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MessengerDialogMessagesWPF.Service
{
    public sealed class MessengerDialogMessagesWPFService : CommonMessengerService
    {
        private MessengerDialogMessagesWPF m_MainControl;
        //--------------------------------------------------------------
        public MessengerDialogMessagesWPFService(MessengerDialogMessagesWPF _MainControl)
        {
            m_MainControl = _MainControl;
        }
        //--------------------------------------------------------------
        public override void AddMessage(Panel _Container, MessengerDialogMessage _Message, bool _HasAttachment)
        {
            m_MainControl.Info.MessageHasAttachment = _HasAttachment;
            MessageTypeWPF _MessageTypeWPF = _Message.MessageTypeWPF;
            Action AddNewMainStackPanelDelegate = new Action(() =>
            {
                _Container.Children.Add(m_MainControl.Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageMainStackPanel,
                    _MessageTypeWPF, new List<MessengerDialogMessage>() { _Message })));
            });

            bool NeedRenderNewMessagesElements = !HasNameFrameworkElementInElements(_Container.Children, "NewMessagesGrid");

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                m_MainControl.Info.MessengerDialog.IsRead = false;
            }
            else if (!NeedRenderNewMessagesElements)
            {
                m_MainControl.HandlebtnCheckMessages_Click();
            }
            else
            {
                m_MainControl.Info.MessengerDialog.IsRead = true;
            }

            if (NeedRenderNewMessagesElements && _MessageTypeWPF == MessageTypeWPF.Income)
            {
                Grid _NewMessagesGrid = (Grid)m_MainControl.Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.NewMessagesGrid));

                m_MainControl.SetNewMessagesGrid_SizeChanged(_NewMessagesGrid);

                _Container.Children.Add(_NewMessagesGrid);

                _Container.Children.Add(m_MainControl.Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.DepartureDateTextBox,
                    _Message.DepartureDate, true)));

                AddNewMainStackPanelDelegate.Invoke();
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Income && m_MainControl.Info.MaxIndexIncomeSP == 0)
            {
                AddNewMainStackPanelDelegate.Invoke();
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing && m_MainControl.Info.MaxIndexOutgoingSP == 0)
            {
                AddNewMainStackPanelDelegate.Invoke();
            }
            else
            {
                FrameworkElement _LastMainStackPanel = GetLastMainStackPanelFromMessageTypeWPF(_MessageTypeWPF, _Container);

                if (_LastMainStackPanel == null)
                {
                    AddNewMainStackPanelDelegate.Invoke();
                }
                else
                {
                    StackPanel findSP = (StackPanel)_LastMainStackPanel;

                    FrameworkElement tempRemovedElement = null;

                    foreach (FrameworkElement element in findSP.Children)
                    {
                        if (element.Name.StartsWith("MessageSourceTextBoxIncome") || element.Name.StartsWith("MessageSourceTextBoxOutgoing"))
                        {
                            tempRemovedElement = element;
                            break;
                        }
                    }

                    findSP.Children.Remove(tempRemovedElement);

                    findSP.Children.Add(m_MainControl.Factory.Create(new RequestInfo(
                        (uint)MessengerDialogMessagesWPFElementTypes.MessageFieldStackPanelInBorder,
                        _MessageTypeWPF,
                        Tuple.Create(_Message, 5))));

                    findSP.Children.Add(tempRemovedElement);
                }
            }

            m_MainControl.ScrollToEndForMainScrollViewer();

            m_MainControl.Info.MessageHasAttachment = false;
        }
        //--------------------------------------------------------------
        public override void UpdateMessage(Panel _Container, MessengerDialogMessage _Message)
        {
            StackPanel findSP = (StackPanel)FindFrameworkElementFromKey(_Container, $"DepartureTimeAndStatusSP{_Message.Id}");

            StackPanel _Parent = (StackPanel)findSP.Parent;

            TextBlock _ProxyAttachment = (TextBlock)FindFrameworkElementFromKey(_Parent, "ProxyAttachment");

            if (_ProxyAttachment != null)
            {
                if (_Message.StatusMessage == MessageStatusTypeWPF.NotDelivered)
                {
                    _ProxyAttachment.Text = "Вложение не загружено!";
                }
                else
                {
                    _Parent.Children.Remove(_ProxyAttachment);

                    foreach (MessengerDialogMessageAttachment _Attachment in _Message.Attachments)
                    {
                        if (IsShowAttachmentFromFileType(_Attachment.Type))
                        {
                            Image _MessageFieldImage = (Image)m_MainControl.Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageFieldImage,
                                Convert.FromBase64String(_Attachment.Data), m_MainControl.Info.FactHeight));

                            m_MainControl.SetImage_PreviewMouseLeftButtonDown(_MessageFieldImage);

                            _Parent.Children.Add(_MessageFieldImage);
                        }
                        else
                        {
                            _Parent.Children.Add(m_MainControl.Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageFieldFileStackPanel,
                                _Attachment)));
                        }
                    }
                }
            }

            _Parent.Children.Remove(findSP);

            _Parent.Children.Add(m_MainControl.Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.DepartureTimeAndStatusStackPanel,
                MessageTypeWPF.Outgoing, _Message)));
        }
        //--------------------------------------------------------------
        public override void Clear(Panel _Container)
        {
            var btnCheckMessages = FindFrameworkElementFromKey(_Container, "btnCheckMessages");

            if (btnCheckMessages != null)
            {
                _Container.Children.Remove(btnCheckMessages);
            }

            var urlHyperLinkStackPanel = FindFrameworkElementFromKey(_Container, "URLHyperLinkStackPanel");

            if (urlHyperLinkStackPanel != null)
            {
                _Container.Children.Remove(urlHyperLinkStackPanel);
            }
        }
        //--------------------------------------------------------------
        public bool MessengerDialogMessageIsExists(Panel _Container, int _MessengerDialogMessageId)
        {
            return FindFrameworkElementFromKey(_Container, $"DepartureTimeAndStatusSP{_MessengerDialogMessageId}") != null;
        }
        //--------------------------------------------------------------
        public void GroupMessagesFromIsNewState(IEnumerable<MessengerDialogMessage> _Messages, Panel _Container)
        {
            IEnumerable<IGrouping<bool, MessengerDialogMessage>> groupedMessages = _Messages.GroupBy(m => m.IsNewMessage);

            foreach (var item in groupedMessages)
            {
                if (item.Key)
                {
                    Grid _NewMessagesGrid = (Grid)m_MainControl.Factory.Create(new RequestInfo(
                        (uint)MessengerDialogMessagesWPFElementTypes.NewMessagesGrid));

                    m_MainControl.SetNewMessagesGrid_SizeChanged(_NewMessagesGrid);

                    _Container.Children.Add(_NewMessagesGrid);
                }

                GroupMessagesFromDepartureDate(item.ToList(), _Container, item.Key);
            }
        }
        //--------------------------------------------------------------
        #region Детали имплементации сервиса
        //--------------------------------------------------------------
        private FrameworkElement GetLastMainStackPanelFromMessageTypeWPF(MessageTypeWPF _MessageTypeWPF, Panel _Container)
        {
            string _IncomeKeyToFind = "IncomeMessageOutgoingSP" + m_MainControl.Info.MaxIndexIncomeSP.ToString();
            string _OutgoingKeyToFind = "OutgoingMessageOutgoingSP" + m_MainControl.Info.MaxIndexOutgoingSP.ToString();
            int _Count = _Container.Children.Count;

            for (int i = _Count - 1; i >= 0; i--)
            {
                UIElement temp = _Container.Children[i];

                if (temp is Panel)
                {
                    FrameworkElement _IncomeFE = FindFrameworkElementFromKey((Panel)temp, _IncomeKeyToFind);
                    FrameworkElement _OutgoingFE = FindFrameworkElementFromKey((Panel)temp, _OutgoingKeyToFind);

                    if (_IncomeFE == null && _OutgoingFE == null)
                    {
                        continue;
                    }
                    else if (_MessageTypeWPF == MessageTypeWPF.Income && _IncomeFE != null)
                    {
                        return _IncomeFE;
                    }
                    else if (_MessageTypeWPF == MessageTypeWPF.Outgoing && _OutgoingFE != null)
                    {
                        return _OutgoingFE;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }
        //--------------------------------------------------------------
        private void GroupMessagesFromDepartureDate(IEnumerable<MessengerDialogMessage> _Messages, Panel _Container, bool _IsNew)
        {
            IEnumerable<IGrouping<string, MessengerDialogMessage>> groupedMessages = _Messages.GroupBy(m => m.DepartureDate);

            foreach (var item in groupedMessages)
            {
                _Container.Children.Add(m_MainControl.Factory.Create(new RequestInfo(
                    (uint)MessengerDialogMessagesWPFElementTypes.DepartureDateTextBox, item.Key, _IsNew)));

                MessageTypeWPF? currentMessageType = null;
                MessageTypeWPF? lastMessageType = null;
                string currentSecUserFIO = null;
                string lastSecUserFIO = null;
                List<MessengerDialogMessage> messages = new List<MessengerDialogMessage>();

                foreach (MessengerDialogMessage message in item)
                {
                    currentMessageType = message.MessageTypeWPF;
                    currentSecUserFIO = message.UserName;

                    if (lastMessageType == null)
                    {
                        messages.Add(message);
                    }
                    else if (currentMessageType == lastMessageType && currentSecUserFIO == lastSecUserFIO)
                    {
                        messages.Add(message);
                    }
                    else
                    {
                        if (messages.Count > 0)
                        {
                            _Container.Children.Add(m_MainControl.Factory.Create(new RequestInfo(
                                (uint)MessengerDialogMessagesWPFElementTypes.MessageMainStackPanel,
                                (MessageTypeWPF)lastMessageType, messages)));
                        }

                        messages.Clear();

                        messages.Add(message);
                    }

                    lastMessageType = message.MessageTypeWPF;
                    lastSecUserFIO = message.UserName;
                }

                if (messages.Count > 0)
                {
                    _Container.Children.Add(m_MainControl.Factory.Create(new RequestInfo(
                        (uint)MessengerDialogMessagesWPFElementTypes.MessageMainStackPanel,
                        (MessageTypeWPF)currentMessageType, messages)));
                }
            }
        }
        //--------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------
    }
}
