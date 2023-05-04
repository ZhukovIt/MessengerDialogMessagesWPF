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

namespace MessengerDialogMessagesWPF
{
    /// <summary>
    /// Логика взаимодействия для MessengerDialogMessagesWPF.xaml
    /// </summary>
    public partial class MessengerDialogMessagesWPF : UserControl
    {
        private Action<byte[]> m_ShowImage;
        private Func<string, byte[]> m_GetBytesForFileType;
        private Action m_SetStateMessageToReaded;
        private double m_FactWidth;
        private double m_FactHeight;
        private Dictionary<string, string> m_HyperLinksDict;
        //--------------------------------------------------------------
        public MessengerDialogMessagesWPF(List<MessengerDialogMessage> _Messages, Action<byte[]> _ShowImage, Func<string, byte[]> _GetBytesForFileType, Action _SetStateMessageToReaded, double _FactWidth, double _FactHeight)
        {
            InitializeComponent();

            m_ShowImage = _ShowImage;

            m_GetBytesForFileType = _GetBytesForFileType;

            m_SetStateMessageToReaded = _SetStateMessageToReaded;

            m_FactWidth = _FactWidth;

            m_FactHeight = _FactHeight;

            m_HyperLinksDict = new Dictionary<string, string>();

            GroupMessagesFromIsNewState(_Messages);

            MainScrollViewer.ScrollToEnd();
        }
        //--------------------------------------------------------------
        public void AddNewMessage(MessengerDialogMessage _Message)
        {
            //spMessages.Children.Add(CreateMessageMainStackPanel(_Message));
        }
        //--------------------------------------------------------------
        #region Вспомогательные закрытые методы и атрибуты
        //--------------------------------------------------------------
        private void GroupMessagesFromIsNewState(IEnumerable<MessengerDialogMessage> _Messages)
        {
            IEnumerable<IGrouping<bool, MessengerDialogMessage>> groupedMessages = _Messages.GroupBy(m => m.IsNewMessage);

            foreach (var item in groupedMessages)
            {
                if (item.Key)
                {
                    spMessages.Children.Add(CreateNewMessagesGrid());
                }

                GroupMessagesFromDepartureDate(item.ToList());

                if (item.Key)
                {
                    spMessages.Children.Add(CreateButtonCheckMessages());
                }
            }
        }
        //--------------------------------------------------------------
        private void GroupMessagesFromDepartureDate(IEnumerable<MessengerDialogMessage> _Messages)
        {
            IEnumerable<IGrouping<string, MessengerDialogMessage>> groupedMessages = _Messages.GroupBy(m => m.DepartureDate);

            foreach (var item in groupedMessages)
            {
                spMessages.Children.Add(CreateDepartureDateTextBox(item.Key));

                MessageTypeWPF? currentMessageType = null;
                MessageTypeWPF? lastMessageType = null;
                List<MessengerDialogMessage> messages = new List<MessengerDialogMessage>();

                foreach (MessengerDialogMessage message in item)
                {
                    currentMessageType = message.MessageTypeWPF;

                    if (lastMessageType == null)
                    {
                        messages.Add(message);
                    }
                    else if (currentMessageType == lastMessageType)
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
                }

                if (messages.Count > 0)
                {
                    spMessages.Children.Add(CreateMessageMainStackPanel((MessageTypeWPF)currentMessageType, messages));
                }
            }
        }
        //--------------------------------------------------------------
        private TextBox CreateDepartureDateTextBox(string _DepartureDate)
        {
            TextBox _ResultControl = new TextBox();

            _ResultControl.Text = _DepartureDate;

            _ResultControl.Template = (ControlTemplate)Resources["tBoxDateTemplate"];

            _ResultControl.IsReadOnly = true;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBox CreateDepartureTimeTextBox(MessageTypeWPF _MessageTypeWPF, string _DepartureDate, string _DepartureTime)
        {
            TextBox _ResultControl = new TextBox();

            _ResultControl.Template = (ControlTemplate)Resources["tBoxDepartureTimeTemplate"];

            _ResultControl.Margin = new Thickness(5, -2, 5, 0);

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Text = $"({_DepartureDate})  {_DepartureTime}";
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

                _ResultControl.Text = $"{_DepartureTime}  ({_DepartureDate})";
            }

            _ResultControl.IsReadOnly = true;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBox CreateMessageSourceTextBox(MessageTypeWPF _MessageTypeWPF, string _MessageSourceView)
        {
            TextBox _ResultControl = new TextBox();

            _ResultControl.Template = (ControlTemplate)Resources["tBoxMessageSourceTemplate"];

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Margin = new Thickness(5, 0, 0, 0);
            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

                _ResultControl.Margin = new Thickness(0, 0, 5, 0);
            }

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
        private StackPanel CreateMessageFieldStackPanel(MessageTypeWPF _MessageTypeWPF, MessengerDialogMessage _Message)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Orientation = Orientation.Vertical;

            TextBox _MessageFieldTextBox = CreateMessageFieldTextBox(_MessageTypeWPF, _Message.TextMessage);

            _ResultControl.Children.Add(_MessageFieldTextBox);

            foreach (MessengerDialogMessageAttachment _Attachment in _Message.Attachments)
            {
                if (IsShowAttachmentFromFileType(_Attachment.Type))
                {
                    Image _MessageFieldImage = CreateMessageFieldImage(Convert.FromBase64String(_Attachment.Data));

                    _ResultControl.Children.Add(_MessageFieldImage);
                }
                else
                {
                    _ResultControl.Children.Add(CreateMessageFieldFileStackPanel(_Attachment));
                }
            }

            _ResultControl.Children.Add(CreateDepartureTimeTextBox(_MessageTypeWPF, _Message.DepartureDate, _Message.DepartureTime));

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
        private Image CreateMessageFieldImage(byte[] _ImageBytes)
        {
            Image _ResultControl = new Image();

            _ResultControl.Width = double.NaN;

            _ResultControl.Height = double.NaN;

            _ResultControl.MaxHeight = m_FactHeight * 2 / 3;

            _ResultControl.Margin = new Thickness(2, 0, 2, 15);

            _ResultControl.Cursor = Cursors.Hand;

            _ResultControl.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Image_PreviewMouseLeftButtonDown);

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
        private StackPanel CreateMessageFieldFileStackPanel(MessengerDialogMessageAttachment _Attachment)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Orientation = Orientation.Horizontal;

            _ResultControl.Margin = new Thickness(0, 0, 0, 10);

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

            Image _FileImage = new Image();

            byte[] imageFileType = m_GetBytesForFileType.Invoke(_Attachment.Type);

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

                if (!m_HyperLinksDict.ContainsKey(fileFullName))
                {
                    m_HyperLinksDict.Add(fileFullName, _Attachment.Data);
                }

                hyperlink.Click += HyperLink_Click;

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
        private Border CreateMessageFieldStackPanelInBorder(MessageTypeWPF _MessageTypeWPF, MessengerDialogMessage _Message)
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

            TextBox _CompanionNameTextBox = null;
            TextBox _MessageSourceTextBox = null;

            if (_MessageTypeWPF == MessageTypeWPF.Income)
            {
                _CompanionNameTextBox = CreateCompanionNameTextBox(_MessageTypeWPF, _Messages[0].ClientName);
                _MessageSourceTextBox = CreateMessageSourceTextBox(_MessageTypeWPF, _Messages[0].MessageSourceClientView);

            }
            else if (_MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _CompanionNameTextBox = CreateCompanionNameTextBox(_MessageTypeWPF, _Messages[0].UserName);
                _MessageSourceTextBox = CreateMessageSourceTextBox(_MessageTypeWPF, _Messages[0].MessageSourceUserView);
            }

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

            Ellipse _ClientImageInEllipse = CreateClientImageInEllipse(_Messages[0].ClientPhoto);

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
        private Grid CreateNewMessagesGrid()
        {
            Grid _ResultControl = new Grid();

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
        private Button CreateButtonCheckMessages()
        {
            Button _ResultControl = new Button();

            _ResultControl.Template = (ControlTemplate)Resources["btnCheckMessages"];

            _ResultControl.Content = "Отметить сообщения как прочитанные";

            _ResultControl.Cursor = Cursors.Hand;

            _ResultControl.Click += new RoutedEventHandler(btnCheckMessages_Click);

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private bool IsShowAttachmentFromFileType(string _FileType)
        {
            switch (_FileType)
            {
                case "jpg":
                case "jpeg":
                case "png":
                case "ico":
                case "gif":
                case "svg":
                case "bmp":
                case "tiff":
                case "photo":
                case "sticker":
                    return true;
                default:
                    return false;
            }
        }
        //--------------------------------------------------------------
        private void Image_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image image = (Image)sender;

            MemoryStream stream = (MemoryStream)((BitmapImage)image.Source).StreamSource;

            m_ShowImage.Invoke(stream.ToArray());
        }
        //--------------------------------------------------------------
        private void HyperLink_Click(object sender, RoutedEventArgs e)
        {
            string hyperlinkText = m_HyperLinksDict[
                ((Run)
                    ((Hyperlink)sender)
                    .Inlines.FirstInline
                ).Text
            ];

            Process.Start(hyperlinkText);
        }
        //--------------------------------------------------------------
        private void btnCheckMessages_Click(object sender, RoutedEventArgs e)
        {
            m_SetStateMessageToReaded.Invoke();
        }
        //--------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------
    }
}
