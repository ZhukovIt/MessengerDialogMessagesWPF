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
        private CommonMessengerService m_MessengerService;
        private AbstractWPFCreator m_Factory;
        private MessengerDialogMessagesWPFInfo m_Info;
        private IEnumerable<UIElement> m_UIElementsForEnabled;
        //--------------------------------------------------------------
        public MessengerDialogMessagesWPFInfo Info
        {
            get
            {
                return m_Info;
            }
        }
        //--------------------------------------------------------------
        public CommonMessengerService MessengerService
        {
            get
            {
                return m_MessengerService;
            }
        }
        //--------------------------------------------------------------
        public AbstractWPFCreator Factory
        {
            get
            {
                return m_Factory;
            }
        }
        //--------------------------------------------------------------
        public int MessengerDialogId
        {
            get
            {
                if (m_Info.MessengerDialog == null)
                {
                    return -1;
                }

                return m_Info.MessengerDialog.Id;
            }
        }
        //--------------------------------------------------------------
        public List<UIElement> UIElementsForEnabled => (List<UIElement>)m_UIElementsForEnabled;
        //--------------------------------------------------------------
        public MessengerDialogMessagesWPF()
        {
            InitializeComponent();
            m_MessengerService = new MessengerDialogMessagesWPFService(this);
            m_UIElementsForEnabled = new List<UIElement>();
        }
        //--------------------------------------------------------------
        public void Init(MessengerDialogWPF _MessengerDialog, Action<byte[]> _ShowImage, Func<string, byte[]> _GetBytesForFileType, 
            Action<bool> _SetStateMessageToReaded, double _FactWidth, double _FactHeight, bool _IsCanDataModify)
        {
            spMessages.Children.Clear();

            FrameworkElement _URLHyperLinkStackPanel = m_MessengerService.FindFrameworkElementFromKey(MainGrid, "URLHyperLinkStackPanel");

            MainGrid.Children.Remove(_URLHyperLinkStackPanel);

            m_Factory = new MessengerDialogMessagesWPFFactory(this, Resources);

            m_Info = new MessengerDialogMessagesWPFInfo(_GetBytesForFileType, _ShowImage, _MessengerDialog, _SetStateMessageToReaded,
                _FactWidth, _FactHeight);

            if (_MessengerDialog.Messages.Count > 0)
            {
                CheckSetURLHyperLinkComment(_MessengerDialog.Messages[0]);
            }

            _MessengerDialog.SetIsReadChangedEventHandler(MessengerDialogIsRead_Changed);

            MessengerDialogIsRead_Changed(_MessengerDialog);

            ((MessengerDialogMessagesWPFService)m_MessengerService).GroupMessagesFromIsNewState(_MessengerDialog.Messages, spMessages);

            ScrollToEndForMainScrollViewer();

            foreach (UIElement _Element in m_UIElementsForEnabled)
            {
                if (_Element != null)
                {
                    _Element.IsEnabled = _IsCanDataModify;
                }
            }
        }
        //--------------------------------------------------------------
        public void AddMessage(MessengerDialogMessage _Message, bool _HasAttachment)
        {
            m_MessengerService.AddMessage(spMessages, _Message, _HasAttachment);
        }
        //--------------------------------------------------------------
        public void UpdateMessage(MessengerDialogMessage _MessengerDialogMessage)
        {
            m_MessengerService.UpdateMessage(spMessages, _MessengerDialogMessage);
        }
        //--------------------------------------------------------------
        public void Clear()
        {
            spMessages.Children.Clear();

            m_MessengerService.Clear(MainGrid);
        }
        //--------------------------------------------------------------
        public void ScrollToEndForMainScrollViewer()
        {
            MainScrollViewer.ScrollToEnd();
        }
        //--------------------------------------------------------------
        public void SetHyperLink_ClickHandler(Hyperlink _Element)
        {
            _Element.Click += HyperLink_Click;
        }
        //--------------------------------------------------------------
        public void SetImage_PreviewMouseLeftButtonDown(Image _Element)
        {
            _Element.PreviewMouseLeftButtonDown += Image_PreviewMouseLeftButtonDown;
        }
        //--------------------------------------------------------------
        public void SetNewMessagesGrid_SizeChanged(Grid _Element)
        {
            _Element.SizeChanged += NewMessagesGrid_SizeChanged;
        }
        //--------------------------------------------------------------
        public void HandlebtnCheckMessages_Click()
        {
            btnCheckMessages_Click(null, null);
        }
        //--------------------------------------------------------------
        #region Обработчики событий
        //--------------------------------------------------------------
        private void btnCheckMessages_Click(object sender, RoutedEventArgs e)
        {
            m_Info.SetStateMessageToReaded.Invoke(false);

            MainGrid.Children.Remove((UIElement)sender);

            m_Info.MessengerDialog.IsRead = true;
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
            if (m_Info.MessengerDialog.IsComment && _FirstMessage.URL != null)
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

            m_Info.ShowImage.Invoke(stream.ToArray());
        }
        //--------------------------------------------------------------
        private void HyperLink_Click(object sender, RoutedEventArgs e)
        {
            string hyperlinkText = m_Info.HyperLinksDict[
                ((Run)
                    ((Hyperlink)sender)
                    .Inlines.FirstInline
                ).Text
            ];

            Process.Start(hyperlinkText);
        }
        //--------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------
    }
}
