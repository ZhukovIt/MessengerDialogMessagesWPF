using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessengerDialogMessagesWPF.Factory
{
    public sealed class MessengerDialogMessagesWPFFactory : AbstractWPFCreator
    {
        private MessengerDialogMessagesWPF m_MainControl;
        //--------------------------------------------------------------
        public MessengerDialogMessagesWPFFactory(MessengerDialogMessagesWPF _MainControl, ResourceDictionary _Resources) : base(_Resources)
        {
            m_MainControl = _MainControl;
        }
        //--------------------------------------------------------------
        public override FrameworkElement Create(RequestInfo _RequestInfo)
        {
            switch ((MessengerDialogMessagesWPFElementTypes)_RequestInfo.ElementType)
            {
                case MessengerDialogMessagesWPFElementTypes.NewMessagesGrid:
                    return CreateNewMessagesGrid();
                case MessengerDialogMessagesWPFElementTypes.DepartureDateTextBox:
                    return CreateDepartureDateTextBox((string)_RequestInfo.lParam, (bool)_RequestInfo.wParam);
                case MessengerDialogMessagesWPFElementTypes.DepartureTimeAndStatusStackPanel:
                    return CreateDepartureTimeAndStatusStackPanel((MessageTypeWPF)_RequestInfo.lParam, (MessengerDialogMessage)_RequestInfo.wParam);
                case MessengerDialogMessagesWPFElementTypes.MessageSourceTextBox:
                    return CreateMessageSourceTextBox((MessageTypeWPF)_RequestInfo.lParam, (Tuple<string, int, int>)_RequestInfo.wParam);
                case MessengerDialogMessagesWPFElementTypes.CompanionNameTextBox:
                    return CreateCompanionNameTextBox((MessageTypeWPF)_RequestInfo.lParam, (string)_RequestInfo.wParam);
                case MessengerDialogMessagesWPFElementTypes.MessageFieldTextBox:
                    return CreateMessageFieldTextBox((MessageTypeWPF)_RequestInfo.lParam, (string)_RequestInfo.wParam);
                case MessengerDialogMessagesWPFElementTypes.ClientImageInEllipse:
                    return CreateClientImageInEllipse((byte[])_RequestInfo.lParam);
                case MessengerDialogMessagesWPFElementTypes.ProxyAttachmentTextBlock:
                    return CreateProxyAttachmentTextBlock();
                case MessengerDialogMessagesWPFElementTypes.MessageFieldImage:
                    return CreateMessageFieldImage((byte[])_RequestInfo.lParam);
                case MessengerDialogMessagesWPFElementTypes.URLHyperLinkTextBlock:
                    return CreateURLHyperLinkTextBlock((string)_RequestInfo.lParam);
                case MessengerDialogMessagesWPFElementTypes.MessageFieldFileStackPanel:
                    return CreateMessageFieldFileStackPanel((MessengerDialogMessageAttachment)_RequestInfo.lParam);
                case MessengerDialogMessagesWPFElementTypes.ButtonCheckMessages:
                    return CreateButtonCheckMessages();
                case MessengerDialogMessagesWPFElementTypes.URLHyperLinkStackPanel:
                    return CreateURLHyperLinkStackPanel((string)_RequestInfo.lParam);
                case MessengerDialogMessagesWPFElementTypes.MessageFieldStackPanel:
                    return CreateMessageFieldStackPanel((MessageTypeWPF)_RequestInfo.lParam,
                        (MessengerDialogMessage)_RequestInfo.wParam);
                case MessengerDialogMessagesWPFElementTypes.MessageFieldStackPanelInBorder:
                    return CreateMessageFieldStackPanelInBorder((MessageTypeWPF)_RequestInfo.lParam,
                        (Tuple<MessengerDialogMessage, int>)_RequestInfo.wParam);
                case MessengerDialogMessagesWPFElementTypes.MessageMainStackPanel:
                    return CreateMessageMainStackPanel((MessageTypeWPF)_RequestInfo.lParam, (List<MessengerDialogMessage>)_RequestInfo.wParam);
                default:
                    throw new NotImplementedException(
                        $"Элемент типа: {(MessengerDialogMessagesWPFElementTypes)_RequestInfo.ElementType} не поддерживается данной фабрикой!");
            }
        }
        //--------------------------------------------------------------
        #region Вспомогательные методы для создания элементов фабрики
        //--------------------------------------------------------------
        private Grid CreateNewMessagesGrid()
        {
            Grid _ResultControl = new Grid();

            _ResultControl.Name = "NewMessagesGrid";

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(100, GridUnitType.Auto);

            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(103);

            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = new GridLength(100, GridUnitType.Auto);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(24);

            _ResultControl.ColumnDefinitions.Add(column1);
            _ResultControl.ColumnDefinitions.Add(column2);
            _ResultControl.ColumnDefinitions.Add(column3);

            _ResultControl.RowDefinitions.Add(row1);

            Line leftLine = new Line();
            Grid.SetRow(leftLine, 0);
            Grid.SetColumn(leftLine, 0);
            leftLine.X1 = 5;
            leftLine.Y1 = 12;
            leftLine.X2 = 370;
            leftLine.Y2 = 12;
            leftLine.Stroke = new SolidColorBrush(Color.FromRgb(0xdc, 0xe1, 0xe6));
            leftLine.StrokeThickness = 1;

            TextBlock textElement = new TextBlock();
            Grid.SetRow(textElement, 0);
            Grid.SetColumn(textElement, 1);
            textElement.HorizontalAlignment = HorizontalAlignment.Center;
            textElement.VerticalAlignment = VerticalAlignment.Center;
            textElement.Padding = new Thickness(5);
            textElement.FontSize = 12;
            textElement.FontFamily = new FontFamily("Times New Roman");
            textElement.Foreground = new SolidColorBrush(Color.FromRgb(0x62, 0x6d, 0x7a));
            textElement.Text = "Новые сообщения";

            Line rightLine = new Line();
            Grid.SetRow(rightLine, 0);
            Grid.SetColumn(rightLine, 2);
            rightLine.X1 = 5;
            rightLine.Y1 = 12;
            rightLine.X2 = 365;
            rightLine.Y2 = 12;
            rightLine.Stroke = new SolidColorBrush(Color.FromRgb(0xdc, 0xe1, 0xe6));
            rightLine.StrokeThickness = 1;

            _ResultControl.Children.Add(leftLine);
            _ResultControl.Children.Add(textElement);
            _ResultControl.Children.Add(rightLine);

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBox CreateDepartureDateTextBox(string _DepartureDate, bool _IsNew)
        {
            TextBox _ResultControl = new TextBox();

            _ResultControl.Name = "DepartureDateTextBox";

            if (_IsNew)
            {
                _ResultControl.Name += "New";
            }

            _ResultControl.Text = _DepartureDate;

            _ResultControl.Template = (ControlTemplate)Resources["tBoxDateTemplate"];

            _ResultControl.IsReadOnly = true;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private StackPanel CreateDepartureTimeAndStatusStackPanel(MessageTypeWPF _MessageTypeWPF, MessengerDialogMessage _Message)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Name = $"DepartureTimeAndStatusSP{_Message.Id}";

            _ResultControl.Orientation = Orientation.Horizontal;

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

            _ResultControl.VerticalAlignment = VerticalAlignment.Center;

            _ResultControl.Margin = new Thickness(5, -2, 5, 0);

            Image _MessageStatusImage = new Image();

            _MessageStatusImage.Width = 16;

            _MessageStatusImage.Height = 16;

            string _FilePath = null;

            switch (_Message.StatusMessage)
            {
                case MessageStatusTypeWPF.InProcessSend:
                    _FilePath = "../../Resources/message_in_process_send16.png";
                    break;
                case MessageStatusTypeWPF.Delivered:
                    _FilePath = "../../Resources/message_delivered16.png";
                    break;
                case MessageStatusTypeWPF.NotDelivered:
                    _FilePath = "../../Resources/message_not_delivered16.png";
                    break;
                default:
                    throw new NotImplementedException($"Данный код не поддерживает MessageStatusTypeWPF = {_Message.StatusMessage}");
            }

            BitmapImage _ImageSource = new BitmapImage(new Uri(_FilePath, UriKind.Relative));

            _MessageStatusImage.Stretch = Stretch.Fill;

            _MessageStatusImage.Source = _ImageSource;

            TextBox _DepartureTimeTextBox = new TextBox();

            _DepartureTimeTextBox.Template = (ControlTemplate)Resources["tBoxDepartureTimeTemplate"];

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _DepartureTimeTextBox.Text = $"({_Message.DepartureDate})  {_Message.DepartureTime}";

                _ResultControl.Children.Add(_DepartureTimeTextBox);

                _ResultControl.Children.Add(_MessageStatusImage);

                _DepartureTimeTextBox.Margin = new Thickness(0, 0, 5, 0);
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

                _DepartureTimeTextBox.Text = $"{_Message.DepartureTime}  ({_Message.DepartureDate})";

                _ResultControl.Children.Add(_MessageStatusImage);

                _ResultControl.Children.Add(_DepartureTimeTextBox);

                _DepartureTimeTextBox.Margin = new Thickness(5, 0, 0, 0);
            }

            _DepartureTimeTextBox.IsReadOnly = true;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBox CreateMessageSourceTextBox(MessageTypeWPF _MessageTypeWPF, Tuple<string, int, int> _DataItems)
        {
            string _MessageSourceView = _DataItems.Item1;
            int _MaxIndexIncomeSP = _DataItems.Item2;
            int _MaxIndexOutgoingSP = _DataItems.Item3;

            TextBox _ResultControl = new TextBox();

            string _Name = "";

            _ResultControl.Template = (ControlTemplate)Resources["tBoxMessageSourceTemplate"];

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Margin = new Thickness(5, 0, 0, 0);

                _Name = $"MessageSourceTextBoxIncome{_MaxIndexIncomeSP}";
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

                _ResultControl.Margin = new Thickness(0, 0, 5, 0);

                _Name = $"MessageSourceTextBoxOutgoing{_MaxIndexOutgoingSP}";
            }

            _ResultControl.Name = _Name;

            _ResultControl.Text = _MessageSourceView;

            _ResultControl.IsReadOnly = true;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBox CreateCompanionNameTextBox(MessageTypeWPF _MessageTypeWPF, string _CompanionName)
        {
            TextBox _ResultControl = new TextBox();

            _ResultControl.Template = (ControlTemplate)Resources["tBoxUserClientNameTemplate"];

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

                _ResultControl.Margin = new Thickness(5, 5, 0, 0);
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

                _ResultControl.Margin = new Thickness(0, 5, 5, 0);
            }

            _ResultControl.Text = _CompanionName;

            _ResultControl.IsReadOnly = true;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBox CreateMessageFieldTextBox(MessageTypeWPF _MessageTypeWPF, string _MessageText)
        {
            TextBox _ResultControl = new TextBox();

            _ResultControl.BorderThickness = new Thickness(0);

            _ResultControl.Width = double.NaN;

            _ResultControl.Height = double.NaN;

            _ResultControl.Padding = new Thickness(10);

            _ResultControl.Margin = new Thickness(0, 0, 5, 0);

            _ResultControl.Background = new SolidColorBrush(Colors.Transparent);

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

                _ResultControl.TextAlignment = TextAlignment.Left;
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

                _ResultControl.TextAlignment = TextAlignment.Right;
            }

            _ResultControl.Text = _MessageText;

            _ResultControl.TextWrapping = TextWrapping.Wrap;

            _ResultControl.IsReadOnly = true;

            _ResultControl.FontSize = 14;

            _ResultControl.FontFamily = new FontFamily("Times New Roman");

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Ellipse CreateClientImageInEllipse(byte[] _ImageBytes)
        {
            Ellipse _ResultControl = new Ellipse();

            _ResultControl.VerticalAlignment = VerticalAlignment.Top;

            _ResultControl.Width = 48;

            _ResultControl.Height = 48;

            _ResultControl.Margin = new Thickness(5, 0, 0, 0);

            ImageBrush _ImageBrush = new ImageBrush();

            MemoryStream stream = new MemoryStream(_ImageBytes);

            BitmapImage imageSource = new BitmapImage();

            imageSource.BeginInit();

            imageSource.StreamSource = stream;

            imageSource.EndInit();

            _ImageBrush.ImageSource = imageSource;

            _ResultControl.Fill = _ImageBrush;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBlock CreateProxyAttachmentTextBlock()
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.Text = "Вложение загружается ...";

            _ResultControl.Name = "ProxyAttachment";

            _ResultControl.FontSize = 14;

            _ResultControl.FontFamily = new FontFamily("Times New Roman");

            _ResultControl.VerticalAlignment = VerticalAlignment.Center;

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Center;

            _ResultControl.Height = 20;

            _ResultControl.Margin = new Thickness(5, 0, 5, 0);

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Image CreateMessageFieldImage(byte[] _ImageBytes)
        {
            Image _ResultControl = new Image();

            _ResultControl.Width = double.NaN;

            _ResultControl.Height = double.NaN;

            _ResultControl.MaxHeight = m_MainControl.Info.FactHeight * 2 / 3;

            _ResultControl.Margin = new Thickness(2, 0, 2, 15);

            _ResultControl.Cursor = Cursors.Hand;

            MemoryStream stream = new MemoryStream(_ImageBytes);

            BitmapImage imageSource = new BitmapImage();

            imageSource.BeginInit();

            imageSource.StreamSource = stream;

            imageSource.EndInit();

            if (imageSource.Height <= _ResultControl.MaxHeight)
            {
                _ResultControl.Stretch = Stretch.None;
            }
            else
            {
                _ResultControl.Stretch = Stretch.Uniform;
            }

            _ResultControl.Source = imageSource;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBlock CreateURLHyperLinkTextBlock(string _MessageURL)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.VerticalAlignment = VerticalAlignment.Center;

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Center;

            _ResultControl.FontSize = 14;

            _ResultControl.FontFamily = new FontFamily("Times New Roman");

            _ResultControl.Margin = new Thickness(10);

            string _Text = "Ссылка на первый комментарий";

            if (!string.IsNullOrEmpty(_MessageURL?.Trim()))
            {
                Hyperlink hyperlink = new Hyperlink();

                hyperlink.Inlines.Add(_Text);

                if (!m_MainControl.Info.HyperLinksDict.ContainsKey(_Text))
                {
                    m_MainControl.Info.HyperLinksDict.Add(_Text, _MessageURL);
                }

                m_MainControl.SetHyperLink_ClickHandler(hyperlink);

                _ResultControl.Inlines.Add(hyperlink);
            }

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private StackPanel CreateMessageFieldFileStackPanel(MessengerDialogMessageAttachment _Attachment)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Orientation = Orientation.Horizontal;

            _ResultControl.Margin = new Thickness(0, 0, 0, 10);

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

            Image _FileImage = new Image();

            byte[] imageFileType = m_MainControl.Info.GetBytesForFileType.Invoke(_Attachment.Type);

            MemoryStream streamFileType = new MemoryStream(imageFileType);

            BitmapImage imageSourceFileType = new BitmapImage();

            imageSourceFileType.BeginInit();

            imageSourceFileType.StreamSource = streamFileType;

            imageSourceFileType.EndInit();

            _FileImage.Source = imageSourceFileType;

            _FileImage.Width = double.NaN;

            _FileImage.Height = 32;

            _FileImage.Margin = new Thickness(10, 0, 0, 0);

            TextBlock _FileNameTextBlock = new TextBlock();

            _FileNameTextBlock.VerticalAlignment = VerticalAlignment.Center;

            _FileNameTextBlock.FontSize = 14;

            _FileNameTextBlock.FontFamily = new FontFamily("Times New Roman");

            _FileNameTextBlock.Margin = new Thickness(0, 0, 10, 0);

            string fileFullName = _Attachment.Name + "." + _Attachment.Type;

            if (!string.IsNullOrEmpty(_Attachment.Data?.Trim()))
            {
                _ResultControl.Cursor = Cursors.Hand;

                Hyperlink hyperlink = new Hyperlink();

                hyperlink.Inlines.Add(fileFullName);

                if (!m_MainControl.Info.HyperLinksDict.ContainsKey(fileFullName))
                {
                    m_MainControl.Info.HyperLinksDict.Add(fileFullName, _Attachment.Data);
                }

                m_MainControl.SetHyperLink_ClickHandler(hyperlink);

                _FileNameTextBlock.Inlines.Add(hyperlink);
            }
            else
            {
                _FileNameTextBlock.Text = $"Файл {fileFullName} отправлен";
            }

            _ResultControl.Children.Add(_FileImage);

            _ResultControl.Children.Add(_FileNameTextBlock);

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Button CreateButtonCheckMessages()
        {
            Button _ResultControl = new Button();

            _ResultControl.Name = "btnCheckMessages";

            _ResultControl.Template = (ControlTemplate)Resources["btnCheckMessages"];

            _ResultControl.Content = "Отметить сообщения как прочитанные";

            _ResultControl.Cursor = Cursors.Hand;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private StackPanel CreateURLHyperLinkStackPanel(string _MessageURL)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Name = "URLHyperLinkStackPanel";

            _ResultControl.Orientation = Orientation.Horizontal;

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Center;

            FrameworkElement _URLHyperLinkTextBlock = Create(new RequestInfo((uint)MessengerDialogMessagesWPFElementTypes.URLHyperLinkTextBlock, _MessageURL));

            _ResultControl.Children.Add(_URLHyperLinkTextBlock);

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private StackPanel CreateMessageFieldStackPanel(MessageTypeWPF _MessageTypeWPF, MessengerDialogMessage _Message)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Name = $"Message{_Message.Id}";

            _ResultControl.Orientation = Orientation.Vertical;

            FrameworkElement _MessageFieldTextBox = CreateMessageFieldTextBox(_MessageTypeWPF, _Message.TextMessage);

            _ResultControl.Children.Add(_MessageFieldTextBox);

            if (m_MainControl.Info.MessageHasAttachment && _Message.NeedProxyAttachment)
            {
                _ResultControl.Children.Add(CreateProxyAttachmentTextBlock());
            }

            foreach (MessengerDialogMessageAttachment _Attachment in _Message.Attachments)
            {
                if (m_MainControl.MessengerService.IsShowAttachmentFromFileType(_Attachment.Type))
                {
                    Image _MessageFieldImage = CreateMessageFieldImage(Convert.FromBase64String(_Attachment.Data));

                    m_MainControl.SetImage_PreviewMouseLeftButtonDown(_MessageFieldImage);

                    _ResultControl.Children.Add(_MessageFieldImage);
                }
                else
                {
                    _ResultControl.Children.Add(CreateMessageFieldFileStackPanel(_Attachment));
                }
            }

            _ResultControl.Children.Add(CreateDepartureTimeAndStatusStackPanel(_MessageTypeWPF, _Message));

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Border CreateMessageFieldStackPanelInBorder(MessageTypeWPF _MessageTypeWPF, Tuple<MessengerDialogMessage, int> _DataItems)
        {
            MessengerDialogMessage _Message = _DataItems.Item1;

            int _MarginBottom = _DataItems.Item2;

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

            _ResultControl.MaxWidth = m_MainControl.Info.FactWidth * 2 / 3;

            if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Margin = new Thickness(0, 0, 5, 0);
            }

            FrameworkElement _CompanionNameTextBox = null;
            FrameworkElement _MessageSourceTextBox = null;

            string _Name = "";

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                m_MainControl.Info.MaxIndexIncomeSP++;
                _Name = $"IncomeMessageOutgoingSP{m_MainControl.Info.MaxIndexIncomeSP}";
                _CompanionNameTextBox = CreateCompanionNameTextBox(_MessageTypeWPF, _Messages[0].ClientName);
                _MessageSourceTextBox = CreateMessageSourceTextBox(_MessageTypeWPF, 
                    Tuple.Create(_Messages[0].MessageSourceClientView, m_MainControl.Info.MaxIndexIncomeSP, m_MainControl.Info.MaxIndexOutgoingSP));
            }

            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                m_MainControl.Info.MaxIndexOutgoingSP++;
                _Name = $"OutgoingMessageOutgoingSP{m_MainControl.Info.MaxIndexOutgoingSP}";
                _CompanionNameTextBox = CreateCompanionNameTextBox(_MessageTypeWPF, _Messages[0].UserName);
                _MessageSourceTextBox = CreateMessageSourceTextBox(_MessageTypeWPF, 
                    Tuple.Create(_Messages[0].MessageSourceUserView, m_MainControl.Info.MaxIndexIncomeSP, m_MainControl.Info.MaxIndexOutgoingSP));
            }

            _ResultControl.Name = _Name;

            for (int i = 0; i < _Messages.Count; i++)
            {
                Border borderMessage = CreateMessageFieldStackPanelInBorder(_MessageTypeWPF,Tuple.Create(_Messages[i], 0));

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

            FrameworkElement _ClientImageInEllipse = CreateClientImageInEllipse(_Messages[0].ClientPhoto);

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
        #endregion
        //--------------------------------------------------------------
    }
}
