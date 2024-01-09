using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessengerDialogMessagesWPF.Factory
{
    public sealed class BackgroundPanelDialogsWPFFactory : AbstractWPFCreator
    {
        public BackgroundPanelDialogsWPFFactory(ResourceDictionary _Resources) : base(_Resources)
        {

        }
        //--------------------------------------------------------------
        public override FrameworkElement Create(RequestInfo _RequestInfo)
        {
            switch ((BackgroundPanelDialogsWPFElementTypes)_RequestInfo.ElementType)
            {
                case BackgroundPanelDialogsWPFElementTypes.DialogSeparatorGrid:
                    return CreateDialogSeparatorGrid();
                case BackgroundPanelDialogsWPFElementTypes.ClientPhotoEllipse:
                    return CreateClientPhotoEllipse((byte[])_RequestInfo.lParam);
                case BackgroundPanelDialogsWPFElementTypes.ClientNameTextBlock:
                    return CreateClientNameTextBlock((string)_RequestInfo.lParam);
                case BackgroundPanelDialogsWPFElementTypes.DialogDateTimeTextBlock:
                    return CreateDialogDateTimeTextBlock((string)_RequestInfo.lParam);
                case BackgroundPanelDialogsWPFElementTypes.MessengerImage:
                    return CreateMessengerImage((byte[])_RequestInfo.lParam);
                case BackgroundPanelDialogsWPFElementTypes.CountMessagesInDialogTextBlock:
                    return CreateCountMessagesInDialogTextBlock((string)_RequestInfo.lParam);
                case BackgroundPanelDialogsWPFElementTypes.CountMessagesInDialogBorder:
                    return CreateCountMessagesInDialogBorder((string)_RequestInfo.lParam);
                case BackgroundPanelDialogsWPFElementTypes.LastMessageTextInDialogTextBlock:
                    return CreateLastMessageTextInDialogTextBlock((string)_RequestInfo.lParam);
                case BackgroundPanelDialogsWPFElementTypes.MessengerDialogGrid:
                    return CreateMessengerDialogGrid((MessengerDialogForBackgroundPanel)_RequestInfo.lParam);
                case BackgroundPanelDialogsWPFElementTypes.GridEmptyMessengerDialogsProxy:
                    return CreateGridEmptyMessengerDialogsProxy();
                default:
                    throw new NotImplementedException(
                        $"Элемент типа: {(BackgroundPanelDialogsWPFElementTypes)_RequestInfo.ElementType} не поддерживается данной фабрикой!");
            }
        }
        //--------------------------------------------------------------
        #region Вспомогательные методы для создания элементов фабрики
        //--------------------------------------------------------------
        private Grid CreateGridEmptyMessengerDialogsProxy()
        {
            Grid _Result = new Grid();

            _Result.Margin = new Thickness(0, 10, 0, 0);

            _Result.Background = new SolidColorBrush(Colors.White);

            _Result.Name = "grdEmptyMessengerDialogsProxy";

            _Result.Height = 280;

            TextBlock _TextBlockEmptyMessengerDialogsProxy = new TextBlock();

            _TextBlockEmptyMessengerDialogsProxy.VerticalAlignment = VerticalAlignment.Center;

            _TextBlockEmptyMessengerDialogsProxy.HorizontalAlignment = HorizontalAlignment.Center;

            _TextBlockEmptyMessengerDialogsProxy.Text = "Нет непрочитанных диалогов!";

            _Result.Children.Add(_TextBlockEmptyMessengerDialogsProxy);

            return _Result;
        }
        //--------------------------------------------------------------
        private Effect CreateDropShadowEffect()
        {
            DropShadowEffect _ResultEffect = new DropShadowEffect();

            _ResultEffect.BlurRadius = 5;

            _ResultEffect.Color = Colors.Black;

            _ResultEffect.ShadowDepth = 2;

            _ResultEffect.Direction = 270;

            _ResultEffect.Opacity = 1;

            return _ResultEffect;
        }
        //--------------------------------------------------------------
        private Grid CreateDialogSeparatorGrid()
        {
            Grid _ResultControl = new Grid();

            _ResultControl.Height = 1;

            _ResultControl.Background = new SolidColorBrush(Colors.White);

            _ResultControl.Effect = CreateDropShadowEffect();

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Ellipse CreateClientPhotoEllipse(byte[] _ImageBytes)
        {
            Ellipse _ResultControl = new Ellipse();

            _ResultControl.Width = 20;

            _ResultControl.Height = 20;

            _ResultControl.VerticalAlignment = VerticalAlignment.Center;

            _ResultControl.Margin = new Thickness(10, 0, 7, 0);

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
        private TextBlock CreateClientNameTextBlock(string _ClientName)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.TextWrapping = TextWrapping.Wrap;

            _ResultControl.FontFamily = new FontFamily("Inter");

            _ResultControl.FontSize = 13;

            _ResultControl.LineHeight = 16;

            _ResultControl.FontStyle = FontStyles.Normal;

            _ResultControl.FontWeight = FontWeights.DemiBold;

            _ResultControl.Margin = new Thickness(0, 0, 0, 1);

            _ResultControl.Text = _ClientName;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBlock CreateDialogDateTimeTextBlock(string _FormatDialogDateTime)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.FontFamily = new FontFamily("Inter");

            _ResultControl.FontSize = 10;

            _ResultControl.LineHeight = 12;

            _ResultControl.FontStyle = FontStyles.Normal;

            _ResultControl.FontWeight = FontWeights.DemiBold;

            _ResultControl.Foreground = new SolidColorBrush(Color.FromRgb(0x69, 0x65, 0x65));

            _ResultControl.Text = _FormatDialogDateTime;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Image CreateMessengerImage(byte[] _ImageBytes)
        {
            Image _ResultControl = new Image();

            _ResultControl.VerticalAlignment = VerticalAlignment.Center;

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

            _ResultControl.Width = 16;

            _ResultControl.Height = 16;

            _ResultControl.Stretch = Stretch.Fill;

            _ResultControl.Margin = new Thickness(0, 0, 8, 0);

            MemoryStream _Stream = new MemoryStream(_ImageBytes);

            BitmapImage _SourceImage = new BitmapImage();

            _SourceImage.BeginInit();

            _SourceImage.StreamSource = _Stream;

            _SourceImage.EndInit();

            _ResultControl.Source = _SourceImage;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBlock CreateCountMessagesInDialogTextBlock(string _CountMessages)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Center;

            _ResultControl.VerticalAlignment = VerticalAlignment.Center;

            _ResultControl.Foreground = new SolidColorBrush(Colors.White);

            _ResultControl.FontFamily = new FontFamily("Inter");

            _ResultControl.FontStyle = FontStyles.Normal;

            _ResultControl.FontWeight = FontWeights.DemiBold;

            _ResultControl.FontSize = 10;

            _ResultControl.LineHeight = 12;

            _ResultControl.Text = _CountMessages;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Border CreateCountMessagesInDialogBorder(string _CountMessages)
        {
            Border _ResultControl = new Border();

            _ResultControl.VerticalAlignment = VerticalAlignment.Center;

            _ResultControl.Margin = new Thickness(0, 0, 8, 0);

            _ResultControl.Width = 18;

            _ResultControl.Height = 18;

            _ResultControl.CornerRadius = new CornerRadius(9);

            _ResultControl.Background = new SolidColorBrush(Color.FromRgb(0x58, 0x7E, 0xDF));

            _ResultControl.HorizontalAlignment = HorizontalAlignment.Right;

            _ResultControl.Child = CreateCountMessagesInDialogTextBlock(_CountMessages);

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private TextBlock CreateLastMessageTextInDialogTextBlock(string _LastMessageText)
        {
            TextBlock _ResultControl = new TextBlock();

            _ResultControl.Margin = new Thickness(10, 5, 1, 4);

            _ResultControl.TextWrapping = TextWrapping.Wrap;

            _ResultControl.FontFamily = new FontFamily("Inter");

            _ResultControl.FontSize = 10;

            _ResultControl.LineHeight = 12.1;

            _ResultControl.FontWeight = FontWeights.Normal;

            _ResultControl.Text = _LastMessageText;

            return _ResultControl;
        }
        //--------------------------------------------------------------
        private Grid CreateMessengerDialogGrid(MessengerDialogForBackgroundPanel _MessengerDialog)
        {
            Grid _ResultControl = new Grid();

            _ResultControl.Cursor = Cursors.Hand;

            _ResultControl.Background = new SolidColorBrush(Color.FromRgb(0xAC, 0xE1, 0xFE));

            _ResultControl.Background.Opacity = 0.38;

            RowDefinition firstRow = new RowDefinition();
            firstRow.Height = new GridLength(4);

            RowDefinition secondRow = new RowDefinition();
            secondRow.Height = new GridLength(100, GridUnitType.Star);

            RowDefinition thirdRow = new RowDefinition();
            thirdRow.Height = new GridLength(12);

            RowDefinition fourthRow = new RowDefinition();
            fourthRow.Height = new GridLength(100, GridUnitType.Star);

            _ResultControl.RowDefinitions.Add(firstRow);
            _ResultControl.RowDefinitions.Add(secondRow);
            _ResultControl.RowDefinitions.Add(thirdRow);
            _ResultControl.RowDefinitions.Add(fourthRow);

            ColumnDefinition firstColumn = new ColumnDefinition();
            firstColumn.Width = new GridLength(37);

            ColumnDefinition secondColumn = new ColumnDefinition();
            secondColumn.Width = new GridLength(100, GridUnitType.Star);

            ColumnDefinition thirdColumn = new ColumnDefinition();
            thirdColumn.Width = new GridLength(26);

            ColumnDefinition fourthColumn = new ColumnDefinition();
            fourthColumn.Width = new GridLength(26);

            _ResultControl.ColumnDefinitions.Add(firstColumn);
            _ResultControl.ColumnDefinitions.Add(secondColumn);
            _ResultControl.ColumnDefinitions.Add(thirdColumn);
            _ResultControl.ColumnDefinitions.Add(fourthColumn);

            Ellipse _ClientPhotoEllipse = CreateClientPhotoEllipse(_MessengerDialog.ClientPhoto);

            Grid.SetRow(_ClientPhotoEllipse, 1);

            Grid.SetColumn(_ClientPhotoEllipse, 0);

            Grid.SetRowSpan(_ClientPhotoEllipse, 2);

            _ResultControl.Children.Add(_ClientPhotoEllipse);

            TextBlock _ClientNameTextBlock = CreateClientNameTextBlock(_MessengerDialog.ClientName);

            Grid.SetRow(_ClientNameTextBlock, 1);

            Grid.SetColumn(_ClientNameTextBlock, 1);

            _ResultControl.Children.Add(_ClientNameTextBlock);

            TextBlock _DialogDateTimeTextBlock = CreateDialogDateTimeTextBlock(_MessengerDialog.DialogDateTime);

            Grid.SetRow(_DialogDateTimeTextBlock, 2);

            Grid.SetColumn(_DialogDateTimeTextBlock, 1);

            _ResultControl.Children.Add(_DialogDateTimeTextBlock);

            Image _MessengerImage = CreateMessengerImage(_MessengerDialog.MessengerImage);

            Grid.SetColumn(_MessengerImage, 2);

            Grid.SetRow(_MessengerImage, 1);

            Grid.SetRowSpan(_MessengerImage, 2);

            _ResultControl.Children.Add(_MessengerImage);

            Border _CountMessagesInDialogBorder = CreateCountMessagesInDialogBorder(_MessengerDialog.CountMessages);

            Grid.SetRow(_CountMessagesInDialogBorder, 1);

            Grid.SetColumn(_CountMessagesInDialogBorder, 3);

            Grid.SetRowSpan(_CountMessagesInDialogBorder, 2);

            _ResultControl.Children.Add(_CountMessagesInDialogBorder);

            TextBlock _LastMessageTextInDialogTextBlock = CreateLastMessageTextInDialogTextBlock(_MessengerDialog.LastMessageText);

            Grid.SetRow(_LastMessageTextInDialogTextBlock, 3);

            Grid.SetColumn(_LastMessageTextInDialogTextBlock, 0);

            Grid.SetColumnSpan(_LastMessageTextInDialogTextBlock, 4);

            _ResultControl.Children.Add(_LastMessageTextInDialogTextBlock);

            _ResultControl.Name = $"MessengerDialogGrid_{_MessengerDialog.MessengerDialogId}";

            return _ResultControl;
        }
        //--------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------
    }
}
