using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Documents;
using System.Collections;
using MessengerDialogMessagesWPF.Service;
using MessengerDialogMessagesWPF.Factory;

namespace MessengerDialogMessagesWPF
{
    /// <summary>
    /// Логика взаимодействия для MessengerDialogMessagesWPF.xaml
    /// </summary>
    public partial class MessengerDialogMessagesWPF : UserControl
    {
        private IMessengerService m_MessengerService;
        private AbstractWPFCreator m_Factory;
        private MessengerDialogWPF m_MessengerDialog;
        private Action<byte[]> m_ShowImage;
        private Action<bool> m_SetStateMessageToReaded;
        private double m_FactWidth;
        private double m_FactHeight;
        private int m_MaxIndexIncomeSP;
        private int m_MaxIndexOutgoingSP;
        private bool m_MessageHasAttachment;
        //--------------------------------------------------------------
        public MessengerDialogMessagesWPF()
        {
            InitializeComponent();
            m_MessengerService = new MessengerDialogMessagesWPFService();
        }
        //--------------------------------------------------------------
        public void Init(MessengerDialogWPF _MessengerDialog, Action<byte[]> _ShowImage, Func<string, byte[]> _GetBytesForFileType, 
            Action<bool> _SetStateMessageToReaded, double _FactWidth, double _FactHeight)
        {
            m_Factory = new MessengerDialogMessagesWPFFactory(Resources, _GetBytesForFileType);

            m_ShowImage = _ShowImage;

            m_MessengerDialog = _MessengerDialog;

            if (_MessengerDialog.Messages.Count > 0)
            {
                CheckSetURLHyperLinkComment(_MessengerDialog.Messages[0]);
            }

            _MessengerDialog.SetIsReadChangedEventHandler(MessengerDialogIsRead_Changed);

            MessengerDialogIsRead_Changed(_MessengerDialog);

            m_SetStateMessageToReaded = _SetStateMessageToReaded;

            m_FactWidth = _FactWidth;

            m_FactHeight = _FactHeight;

            m_MaxIndexIncomeSP = 0;

            m_MaxIndexOutgoingSP = 0;

            GroupMessagesFromIsNewState(_MessengerDialog.Messages);

            MainScrollViewer.ScrollToEnd();
        }
        //--------------------------------------------------------------
        public void AddMessage(MessengerDialogMessage _Message, bool _HasAttachment)
        {
            m_MessageHasAttachment = _HasAttachment;
            MessageTypeWPF _MessageTypeWPF = _Message.MessageTypeWPF;
            Action AddNewMainStackPanelDelegate = new Action(() =>
            {
                spMessages.Children.Add(CreateMessageMainStackPanel(_MessageTypeWPF, new List<MessengerDialogMessage>() { _Message }));
            });

            bool NeedRenderNewMessagesElements = !HasNewMessagesGrid(spMessages.Children);

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                m_MessengerDialog.IsRead = false;
            }
            else if (!NeedRenderNewMessagesElements)
            {
                btnCheckMessages_Click(null, null);
            }
            else
            {
                m_MessengerDialog.IsRead = true;
            }

            if (NeedRenderNewMessagesElements && _MessageTypeWPF == MessageTypeWPF.Income)
            {
                Grid _NewMessagesGrid = (Grid)m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.NewMessagesGrid));

                _NewMessagesGrid.SizeChanged += NewMessagesGrid_SizeChanged;

                spMessages.Children.Add(_NewMessagesGrid);

                spMessages.Children.Add(m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.DepartureDateTextBox, 
                    _Message.DepartureDate, true)));

                AddNewMainStackPanelDelegate.Invoke();
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Income && m_MaxIndexIncomeSP == 0)
            {
                AddNewMainStackPanelDelegate.Invoke();
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing && m_MaxIndexOutgoingSP == 0)
            {
                AddNewMainStackPanelDelegate.Invoke();
            }
            else
            {
                FrameworkElement _LastMainStackPanel = GetLastMainStackPanelFromMessageTypeWPF(_MessageTypeWPF);

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

                    findSP.Children.Add(CreateMessageFieldStackPanelInBorder(_MessageTypeWPF, _Message, 5));

                    findSP.Children.Add(tempRemovedElement);
                }
            }

            MainScrollViewer.ScrollToEnd();

            m_MessageHasAttachment = false;
        }
        //--------------------------------------------------------------
        public void UpdateMessage(MessengerDialogMessage _MessengerDialogMessage)
        {
            StackPanel findSP = (StackPanel)m_MessengerService.FindFrameworkElementFromKey(spMessages, $"DepartureTimeAndStatusSP{_MessengerDialogMessage.Id}");

            StackPanel _Parent = (StackPanel)findSP.Parent;

            TextBlock _ProxyAttachment = (TextBlock)m_MessengerService.FindFrameworkElementFromKey(_Parent, "ProxyAttachment");

            if (_ProxyAttachment != null)
            {
                if (_MessengerDialogMessage.StatusMessage == MessageStatusTypeWPF.NotDelivered)
                {
                    _ProxyAttachment.Text = "Вложение не загружено!";
                }
                else
                {
                    _Parent.Children.Remove(_ProxyAttachment);

                    foreach (MessengerDialogMessageAttachment _Attachment in _MessengerDialogMessage.Attachments)
                    {
                        if (((MessengerDialogMessagesWPFService)m_MessengerService).IsShowAttachmentFromFileType(_Attachment.Type))
                        {
                            Image _MessageFieldImage = (Image)m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageFieldImage,
                                Convert.FromBase64String(_Attachment.Data), m_FactHeight));

                            _MessageFieldImage.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Image_PreviewMouseLeftButtonDown);

                            _Parent.Children.Add(_MessageFieldImage);
                        }
                        else
                        {
                            _Parent.Children.Add(m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageFieldFileStackPanel,
                                _Attachment)));
                        }
                    }
                }
            }

            _Parent.Children.Remove(findSP);

            _Parent.Children.Add(m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.DepartureTimeAndStatusStackPanel, 
                MessageTypeWPF.Outgoing, _MessengerDialogMessage)));
        }
        //--------------------------------------------------------------
        public void Delete()
        {
            spMessages.Children.Clear();

            var btnCheckMessages = m_MessengerService.FindFrameworkElementFromKey(MainGrid, "btnCheckMessages");

            if (btnCheckMessages != null)
            {
                MainGrid.Children.Remove(btnCheckMessages);
            }

            var urlHyperLinkStackPanel = m_MessengerService.FindFrameworkElementFromKey(MainGrid, "URLHyperLinkStackPanel");

            if (urlHyperLinkStackPanel != null)
            {
                MainGrid.Children.Remove(urlHyperLinkStackPanel);
            }
        }
        //--------------------------------------------------------------
        #region Вспомогательные закрытые методы и атрибуты
        //--------------------------------------------------------------
        private FrameworkElement GetLastMainStackPanelFromMessageTypeWPF(MessageTypeWPF _MessageTypeWPF)
        {
            string _IncomeKeyToFind = "IncomeMessageOutgoingSP" + m_MaxIndexIncomeSP.ToString();
            string _OutgoingKeyToFind = "OutgoingMessageOutgoingSP" + m_MaxIndexOutgoingSP.ToString();
            int _Count = spMessages.Children.Count;

            for (int i = _Count - 1; i >= 0; i--)
            {
                UIElement temp = spMessages.Children[i];

                if (temp is Panel)
                {
                    FrameworkElement _IncomeFE = m_MessengerService.FindFrameworkElementFromKey((Panel)temp, _IncomeKeyToFind);
                    FrameworkElement _OutgoingFE = m_MessengerService.FindFrameworkElementFromKey((Panel)temp, _OutgoingKeyToFind);

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
        private bool HasNewMessagesGrid(IEnumerable elements)
        {
            foreach (FrameworkElement element in elements)
            {
                if (element.Name == "NewMessagesGrid")
                {
                    return true;
                }
            }

            return false;
        }
        //--------------------------------------------------------------
        private void GroupMessagesFromIsNewState(IEnumerable<MessengerDialogMessage> _Messages)
        {
            IEnumerable<IGrouping<bool, MessengerDialogMessage>> groupedMessages = _Messages.GroupBy(m => m.IsNewMessage);

            foreach (var item in groupedMessages)
            {
                if (item.Key)
                {
                    Grid _NewMessagesGrid = (Grid)m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.NewMessagesGrid));

                    _NewMessagesGrid.SizeChanged += NewMessagesGrid_SizeChanged;

                    spMessages.Children.Add(_NewMessagesGrid);
                }

                GroupMessagesFromDepartureDate(item.ToList(), item.Key);
            }
        }
        //--------------------------------------------------------------
        private void GroupMessagesFromDepartureDate(IEnumerable<MessengerDialogMessage> _Messages, bool _IsNew)
        {
            IEnumerable<IGrouping<string, MessengerDialogMessage>> groupedMessages = _Messages.GroupBy(m => m.DepartureDate);

            foreach (var item in groupedMessages)
            {
                spMessages.Children.Add(m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.DepartureDateTextBox, item.Key, _IsNew)));

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
                            spMessages.Children.Add(CreateMessageMainStackPanel((MessageTypeWPF)lastMessageType, messages));
                        }

                        messages.Clear();

                        messages.Add(message);
                    }

                    lastMessageType = message.MessageTypeWPF;
                    lastSecUserFIO = message.UserName;
                }

                if (messages.Count > 0)
                {
                    spMessages.Children.Add(CreateMessageMainStackPanel((MessageTypeWPF)currentMessageType, messages));
                }
            }
        }
        //--------------------------------------------------------------
        private StackPanel CreateMessageFieldStackPanel(MessageTypeWPF _MessageTypeWPF, MessengerDialogMessage _Message)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Name = $"Message{_Message.Id}";

            _ResultControl.Orientation = Orientation.Vertical;

            FrameworkElement _MessageFieldTextBox = m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageFieldTextBox, 
                _MessageTypeWPF, _Message.TextMessage));

            _ResultControl.Children.Add(_MessageFieldTextBox);

            if (m_MessageHasAttachment && _Message.NeedProxyAttachment)
            {
                _ResultControl.Children.Add(m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.ProxyAttachmentTextBlock)));
            }

            foreach (MessengerDialogMessageAttachment _Attachment in _Message.Attachments)
            {
                if (((MessengerDialogMessagesWPFService)m_MessengerService).IsShowAttachmentFromFileType(_Attachment.Type))
                {
                    Image _MessageFieldImage = (Image)m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageFieldImage,
                                Convert.FromBase64String(_Attachment.Data), m_FactHeight));

                    _MessageFieldImage.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Image_PreviewMouseLeftButtonDown);

                    _ResultControl.Children.Add(_MessageFieldImage);
                }
                else
                {
                    _ResultControl.Children.Add(m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageFieldFileStackPanel,
                        _Attachment)));
                }
            }

            _ResultControl.Children.Add(m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.DepartureTimeAndStatusStackPanel, 
                _MessageTypeWPF, _Message)));

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Border CreateMessageFieldStackPanelInBorder(MessageTypeWPF _MessageTypeWPF, MessengerDialogMessage _Message, int _MarginBottom = 0)
        {
            Border _ResultControl = new Border();

            _ResultControl.CornerRadius = new CornerRadius(12);

            _ResultControl.BorderThickness = new Thickness(1);

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Background = new SolidColorBrush(Color.FromRgb(0xe2, 0xf4, 0xdb));

                _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Background = new SolidColorBrush(Color.FromRgb(0xda, 0xef, 0xf4));

                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;
            }

            _ResultControl.Child = CreateMessageFieldStackPanel(_MessageTypeWPF, _Message);

            _ResultControl.Padding = new Thickness(0, 0, 0, 5);

            _ResultControl.Margin = new Thickness(0, 0, 0, _MarginBottom);

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private StackPanel CreateMessageOutgoingStackPanel(MessageTypeWPF _MessageTypeWPF, List<MessengerDialogMessage> _Messages)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Orientation = Orientation.Vertical;

            _ResultControl.MaxWidth = m_FactWidth * 2 / 3;

            if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Margin = new Thickness(0, 0, 5, 0);
            }

            FrameworkElement _CompanionNameTextBox = null;
            FrameworkElement _MessageSourceTextBox = null;

            string _Name = "";

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                m_MaxIndexIncomeSP++;
                _Name = $"IncomeMessageOutgoingSP{m_MaxIndexIncomeSP}";
                _CompanionNameTextBox = m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.CompanionNameTextBox, 
                    _MessageTypeWPF, _Messages[0].ClientName));
                _MessageSourceTextBox = m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageSourceTextBox,
                    _MessageTypeWPF, Tuple.Create(_Messages[0].MessageSourceClientView, m_MaxIndexIncomeSP, m_MaxIndexOutgoingSP)));
            }

            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                m_MaxIndexOutgoingSP++;
                _Name = $"OutgoingMessageOutgoingSP{m_MaxIndexOutgoingSP}";
                _CompanionNameTextBox = m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.CompanionNameTextBox,
                    _MessageTypeWPF, _Messages[0].UserName));
                _MessageSourceTextBox = m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.MessageSourceTextBox,
                    _MessageTypeWPF, Tuple.Create(_Messages[0].MessageSourceUserView, m_MaxIndexIncomeSP, m_MaxIndexOutgoingSP)));
            }

            _ResultControl.Name = _Name;

            for (int i = 0; i < _Messages.Count; i++)
            {
                Border borderMessage = CreateMessageFieldStackPanelInBorder(_MessageTypeWPF, _Messages[i]);

                borderMessage.Margin = new Thickness(0, 0, 0, 5);

                if (i == 0)
                {
                    _ResultControl.Children.Add(_CompanionNameTextBox);
                }

                _ResultControl.Children.Add(borderMessage);

                if (i == _Messages.Count - 1)
                {
                    _ResultControl.Children.Add(_MessageSourceTextBox);
                }
            }

            return _ResultControl;

        }
        //--------------------------------------------------------------
        private StackPanel CreateMessageMainStackPanel(MessageTypeWPF _MessageTypeWPF, List<MessengerDialogMessage> _Messages)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Orientation = Orientation.Horizontal;

            FrameworkElement _ClientImageInEllipse = m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.ClientImageInEllipse,
                _Messages[0].ClientPhoto));

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Children.Add(_ClientImageInEllipse);

                _ResultControl.Children.Add(CreateMessageOutgoingStackPanel(_MessageTypeWPF, _Messages));
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

                _ResultControl.Children.Add(CreateMessageOutgoingStackPanel(_MessageTypeWPF, _Messages));
            }

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private void btnCheckMessages_Click(object sender, RoutedEventArgs e)
        {
            m_SetStateMessageToReaded.Invoke(true);

            MainGrid.Children.Remove((UIElement)sender);

            m_MessengerDialog.IsRead = true;
        }
        //--------------------------------------------------------------
        private void MessengerDialogIsRead_Changed(MessengerDialogWPF _Sender)
        {
            if (_Sender.IsRead)
            {
                FrameworkElement btnCheckMessages = m_MessengerService.FindFrameworkElementFromKey(MainGrid, "btnCheckMessages");

                if (btnCheckMessages != null)
                {
                    MainGrid.Children.Remove(btnCheckMessages);
                }
            }
            else
            {
                if (m_MessengerService.FindFrameworkElementFromKey(MainGrid, "btnCheckMessages") == null)
                {
                    Button btnCheckMessages = (Button)m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.ButtonCheckMessages));
                    btnCheckMessages.Click += new RoutedEventHandler(btnCheckMessages_Click);

                    MainGrid.Children.Add(btnCheckMessages);

                    Grid.SetRow(btnCheckMessages, 1);
                }
            }
        }
        //--------------------------------------------------------------
        private void CheckSetURLHyperLinkComment(MessengerDialogMessage _FirstMessage)
        {
            if (m_MessengerDialog.IsComment && _FirstMessage.URL != null)
            {
                FrameworkElement _URLHyperLinkStackPanel = m_Factory.Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.URLHyperLinkStackPanel,
                    _FirstMessage.URL));

                MainGrid.Children.Add(_URLHyperLinkStackPanel);

                Grid.SetRow(_URLHyperLinkStackPanel, 2);
            }
        }
        //--------------------------------------------------------------
        private void NewMessagesGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Grid _Grid = (Grid)sender;

            Line _Left = (Line)_Grid.Children[0];
            Line _Right = (Line)_Grid.Children[2];

            if (e.PreviousSize.Width == 0)
            {
                double correctWidth = (e.NewSize.Width - 113) / 2;

                _Left.X2 = correctWidth;
                _Right.X2 = correctWidth + 5;
            }
            else
            {
                double dWidth = (e.NewSize.Width - e.PreviousSize.Width) / 2;

                _Left.X2 += dWidth;
                _Right.X2 += dWidth;
            }
        }
        //--------------------------------------------------------------
        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;

            MemoryStream stream = (MemoryStream)((BitmapImage)image.Source).StreamSource;

            m_ShowImage.Invoke(stream.ToArray());
        }
        //--------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------
    }
}
