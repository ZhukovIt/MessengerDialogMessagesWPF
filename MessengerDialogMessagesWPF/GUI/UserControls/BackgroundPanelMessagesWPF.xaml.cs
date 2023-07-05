using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessengerDialogMessagesWPF.Factory;
using MessengerDialogMessagesWPF.Service;

namespace MessengerDialogMessagesWPF
{
    /// <summary>
    /// Логика взаимодействия для BackgroundPanelMessagesWPF.xaml
    /// </summary>
    public partial class BackgroundPanelMessagesWPF : UserControl
    {
        private CommonMessengerService m_MessengerService;
        private AbstractWPFCreator m_Factory;
        private Action m_SetBackgroundPanelDialogsWPF;
        private Action<int, string> m_SendMessage;
        private Action m_btnClose_Click;
        private Action m_ExpandWindow_Click;
        private MessengerDialogForBackgroundPanel m_MessengerDialog;
        //-----------------------------------------------------------------------------------
        public CommonMessengerService MessengerService
        {
            get
            {
                return m_MessengerService;
            }
        }
        //-----------------------------------------------------------------------------------
        public AbstractWPFCreator Factory
        {
            get
            {
                return m_Factory;
            }
        }
        //-----------------------------------------------------------------------------------
        public int MessengerDialogId
        {
            get
            {
                return m_MessengerDialog.MessengerDialogId;
            }
        }
        //-----------------------------------------------------------------------------------
        public BackgroundPanelMessagesWPF()
        {
            InitializeComponent();
            m_MessengerService = new BackgroundPanelMessagesWPFService(this);
        }
        //-----------------------------------------------------------------------------------
        public void Init(MessengerDialogForBackgroundPanel _MessengerDialog, IEnumerable<MessengerDialogMessage> _Messages, 
            Tuple<
                Action,
                Action<int, string>,
                Action,
                Action> _Handlers)
        {
            m_MessengerDialog = _MessengerDialog;

            m_Factory = new BackgroundPanelMessagesWPFFactory(this, Resources);

            CorrectClientData();

            m_SetBackgroundPanelDialogsWPF = _Handlers.Item1;
            m_SendMessage = _Handlers.Item2;
            m_btnClose_Click = _Handlers.Item3;
            m_ExpandWindow_Click = _Handlers.Item4;

            foreach (var _Message in _Messages)
            {
                StackPanel _MainMessageStackPanel = (StackPanel)m_Factory
                    .Create(new RequestInfo((uint)BackgroundPanelMessagesWPFElementTypes.MainMessageStackPanel, _Message));

                spMessages.Children.Add(_MainMessageStackPanel);
            }
        }
        //-----------------------------------------------------------------------------------
        public void AddNewMessage(MessengerDialogMessage _Message)
        {
            m_MessengerService.AddMessage(spMessages, _Message, false);

            MainScrollViewer.ScrollToEnd();
        }
        //-----------------------------------------------------------------------------------
        public void UpdateMessage(MessengerDialogMessage _Message)
        {
            m_MessengerService.UpdateMessage(spMessages, _Message);
        }
        //-----------------------------------------------------------------------------------
        private void CorrectClientData()
        {
            m_Factory.Create(new RequestInfo((uint)BackgroundPanelMessagesWPFElementTypes.CorrectDataClientPhotoEllipse,
                m_MessengerDialog.ClientPhoto, ClientPhotoEllipse));

            ClientNameTextBox.Text = m_MessengerDialog.ClientName;
        }
        //-----------------------------------------------------------------------------------
        private void btnSetBackgroundPanelDialogWPF_Click(object sender, RoutedEventArgs e)
        {
            m_SetBackgroundPanelDialogsWPF.Invoke();
        }
        //-----------------------------------------------------------------------------------
        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            m_SendMessage.Invoke(MessengerDialogId, tBoxMessageContent.Text);

            tBoxMessageContent.Text = "";

            MainScrollViewer.ScrollToEnd();
        }
        //-----------------------------------------------------------------------------------
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            m_btnClose_Click.Invoke();
        }
        //-----------------------------------------------------------------------------------
        private void btnExpandWindow_Click(object sender, RoutedEventArgs e)
        {
            m_ExpandWindow_Click.Invoke();
        }
        //-----------------------------------------------------------------------------------
    }
}
