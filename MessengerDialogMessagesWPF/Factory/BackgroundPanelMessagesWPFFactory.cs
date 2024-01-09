using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessengerDialogMessagesWPF.Factory
{
    public sealed class BackgroundPanelMessagesWPFFactory : AbstractWPFCreator
    {
        private BackgroundPanelMessagesWPF m_MainControl;
        //-----------------------------------------------------------------------------------
        public BackgroundPanelMessagesWPFFactory(BackgroundPanelMessagesWPF _MainControl, ResourceDictionary _Resources) : base(_Resources)
        {
            m_MainControl = _MainControl;
        }
        //-----------------------------------------------------------------------------------
        public override FrameworkElement Create(RequestInfo _RequestInfo)
        {
            switch ((BackgroundPanelMessagesWPFElementTypes)_RequestInfo.ElementType)
            {
                case BackgroundPanelMessagesWPFElementTypes.BorderDepartureDate:
                    return CreateBorderDepartureDate((string)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.DepartureDateTextBlock:
                    return CreateDepartureDateTextBlock((string)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.StatusMessageImage:
                    return CreateStatusMessageImage((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.MessageSenderDataTextBlock:
                    return CreateMessageSenderDataTextBlock((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.MessageStatusFooterStackPanel:
                    return CreateMessageStatusFooterStackPanel((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.TextMessageTextBlock:
                    return CreateTextMessageTextBlock((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.SecondaryMessageStackPanel:
                    return CreateSecondaryMessageStackPanel((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.SecondaryMessageBorder:
                    return CreateSecondaryMessageBorder((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.SenderNameTextBlock:
                    return CreateSenderNameTextBlock((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.SenderServiceInfoTextBlock:
                    return CreateSenderServiceInfoTextBlock((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.MainMessageStackPanel:
                    return CreateMainMessageStackPanel((MessengerDialogMessage)_RequestInfo.lParam);
                case BackgroundPanelMessagesWPFElementTypes.CorrectDataClientPhotoEllipse:
                    return CorrectDataClientPhotoEllipse((byte[])_RequestInfo.lParam, (Ellipse)_RequestInfo.wParam);
                case BackgroundPanelMessagesWPFElementTypes.CorrectDataMessengerIcon:
                    return CorrectDataMessengerIcon((byte[])_RequestInfo.lParam, (Image)_RequestInfo.wParam);
                default:
                    throw new NotImplementedException(
                        $"Элемент типа: {(BackgroundPanelDialogsWPFElementTypes)_RequestInfo.ElementType} не поддерживается данной фабрикой!");
            }
        }
        //-----------------------------------------------------------------------------------
        private Border CreateBorderDepartureDate(string _DepartureDateText)
        {
            Border _ResultControl = new Border();

            _ResultControl.Style = (Style)Resources["borderDepartureDate"];

            _ResultControl.Child = CreateDepartureDateTextBlock(_DepartureDateText);

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private TextBlock CreateDepartureDateTextBlock(string _DepartureDateText)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.Style = (Style)Resources["tBlockDepartureDate"];

            _ResultControl.Text = _DepartureDateText;

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private Image CreateStatusMessageImage(MessengerDialogMessage _Message)
        {
            Image _ResultControl = new Image();

            _ResultControl.Width = 16;

            _ResultControl.Height = 16;

            if (_Message.MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Margin = new Thickness(0, 0, 15, 0);
            }
            else if (_Message.MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Margin = new Thickness(15, 0, 0, 0);
            }

            _ResultControl.Stretch = Stretch.Fill;

            string _FilePath = m_MainControl.MessengerService.GetFilePathFromMessageStatusTypeWPF(_Message.StatusMessage);

            BitmapImage _ImageSource = new BitmapImage(new Uri(_FilePath, UriKind.Relative));

            _ResultControl.Stretch = Stretch.Fill;

            _ResultControl.Source = _ImageSource;

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private TextBlock CreateMessageSenderDataTextBlock(MessengerDialogMessage _Message)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.Style = (Style)Resources["tBlockMessageSenderData"];

            _ResultControl.VerticalAlignment = VerticalAlignment.Bottom;

            _ResultControl.Foreground = new SolidColorBrush(Color.FromRgb(0xAB, 0xAB, 0xAB));

            if (_Message.MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Margin = new Thickness(0, 0, 5, 3);
            }
            else if (_Message.MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Margin = new Thickness(5, 0, 0, 3);
            }

            _ResultControl.Text = _Message.ShortDepartureDateTime;

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private StackPanel CreateMessageStatusFooterStackPanel(MessengerDialogMessage _Message)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Orientation = Orientation.Horizontal;

            if (_Message.MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

                _ResultControl.Children.Add(CreateMessageSenderDataTextBlock(_Message));

                _ResultControl.Children.Add(CreateStatusMessageImage(_Message));
            }
            else if (_Message.MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

                _ResultControl.Children.Add(CreateStatusMessageImage(_Message));

                _ResultControl.Children.Add(CreateMessageSenderDataTextBlock(_Message));
            }

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private TextBlock CreateTextMessageTextBlock(MessengerDialogMessage _Message)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.Style = (Style)Resources["tBlockMessageText"];

            _ResultControl.Text = _Message.TextMessage;

            if (_Message.MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Margin = new Thickness(10, 5, 20, 5);
            }
            else if(_Message.MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Margin = new Thickness(20, 5, 10, 5);
            }

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private StackPanel CreateSecondaryMessageStackPanel(MessengerDialogMessage _Message)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Name = $"SecondaryMessageStackPanel_{_Message.Id}";

            _ResultControl.Children.Add(CreateTextMessageTextBlock(_Message));

            _ResultControl.Children.Add(CreateMessageStatusFooterStackPanel(_Message));

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private Border CreateSecondaryMessageBorder(MessengerDialogMessage _Message)
        {
            Border _ResultControl = new Border();

            _ResultControl.Child = CreateSecondaryMessageStackPanel(_Message);

            if (_Message.MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Style = (Style)Resources["borderClientMessage"];
            }
            else if (_Message.MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Style = (Style)Resources["borderUserMessage"];
            }

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private TextBlock CreateSenderNameTextBlock(MessengerDialogMessage _Message)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.Style = (Style)Resources["tBlockMessagePreview"];

            if (_Message.MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Margin = new Thickness(8, 5, 0, 4);

                _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

                _ResultControl.Text = _Message.ClientName;
            }
            else if (_Message.MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Margin = new Thickness(0, 5, 8, 4);

                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

                _ResultControl.Text = _Message.UserName;
            }

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private TextBlock CreateSenderServiceInfoTextBlock(MessengerDialogMessage _Message)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.Style = (Style)Resources["tBlockMessageSenderData"];

            _ResultControl.Foreground = new SolidColorBrush(Color.FromRgb(0xA6, 0xA6, 0xA6));

            if (_Message.MessageTypeWPF == MessageTypeWPF.Income)
            {
                _ResultControl.Margin = new Thickness(6, 2, 0, 2);

                _ResultControl.HorizontalAlignment = HorizontalAlignment.Left;

                _ResultControl.Text = _Message.MessageSourceClientView;
            }
            else if (_Message.MessageTypeWPF == MessageTypeWPF.Outgoing)
            {
                _ResultControl.Margin = new Thickness(0, 2, 6, 2);

                _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

                _ResultControl.Text = _Message.MessageSourceUserView;
            }

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private StackPanel CreateMainMessageStackPanel(MessengerDialogMessage _Message)
        {
            StackPanel _ResultControl = new StackPanel();

            _ResultControl.Name = $"spMessage_{_Message.Id}";

            _ResultControl.Children.Add(CreateSenderNameTextBlock(_Message));

            _ResultControl.Children.Add(CreateSecondaryMessageBorder(_Message));

            //_ResultControl.Children.Add(CreateSenderServiceInfoTextBlock(_Message));

            return _ResultControl;
        }
        //-----------------------------------------------------------------------------------
        private Ellipse CorrectDataClientPhotoEllipse(byte[] _ImageBytes, Ellipse _Control)
        {
            ImageBrush _ImageBrush = new ImageBrush();

            MemoryStream stream = new MemoryStream(_ImageBytes);

            BitmapImage _ImageSource = new BitmapImage();

            _ImageSource.BeginInit();

            _ImageSource.StreamSource = stream;

            _ImageSource.EndInit();

            _ImageBrush.ImageSource = _ImageSource;

            _Control.Fill = _ImageBrush;

            return _Control;
        }
        //-----------------------------------------------------------------------------------
        private Image CorrectDataMessengerIcon(byte[] _ImageBytes, Image _Image)
        {
            MemoryStream stream = new MemoryStream(_ImageBytes);

            BitmapImage imageSource = new BitmapImage();

            imageSource.BeginInit();

            imageSource.StreamSource = stream;

            imageSource.EndInit();

            _Image.Source = imageSource;

            return _Image;
        }
        //-----------------------------------------------------------------------------------
    }
}
