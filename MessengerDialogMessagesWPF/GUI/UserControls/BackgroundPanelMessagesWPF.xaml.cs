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
        private Action<int> m_SetBackgroundPanelDialogsWPF;
        private Action<int, string> m_SendMessage;
        private Action m_btnClose_Click;
        private Action m_ExpandWindow_Click;
        private MessengerDialogForBackgroundPanel m_MessengerDialog;
        private Func<int, bool, CheckSecUser> m_CheckSecUser;
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
                Action<int>,
                Action<int, string>,
                Action,
                Action,
                Func<int, bool, CheckSecUser>> _Handlers)
        {
            m_MessengerDialog = _MessengerDialog;

            tBoxMessageContent.MaxLength = m_MessengerDialog.MaxLengthForMessage;

            m_Factory = new BackgroundPanelMessagesWPFFactory(this, Resources);

            CorrectClientData();

            m_SetBackgroundPanelDialogsWPF = _Handlers.Item1;
            m_SendMessage = _Handlers.Item2;
            m_btnClose_Click = _Handlers.Item3;
            m_ExpandWindow_Click = _Handlers.Item4;
            m_CheckSecUser = _Handlers.Item5;

            CheckSecUser _CheckSecUser = m_CheckSecUser.Invoke(MessengerDialogId, true);

            SetCheckSecurityUser(_CheckSecUser.CheckSecurityUserType, _CheckSecUser.SecUserName);

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
        public void SetClientPhoto(byte[] _ImageBytes)
        {
            m_Factory.Create(new RequestInfo((uint)BackgroundPanelMessagesWPFElementTypes.CorrectDataClientPhotoEllipse,
                _ImageBytes, ClientPhotoEllipse));
        }
        //-----------------------------------------------------------------------------------
        private void CorrectClientData()
        {
            SetClientPhoto(m_MessengerDialog.ClientPhoto);

            ClientNameTextBox.Text = m_MessengerDialog.ClientName;
        }
        //-----------------------------------------------------------------------------------
        private void SetCheckSecurityUser(CheckSecurityUserTypes _CheckSecUserType, string _SecUserName = "")
        {
            switch (_CheckSecUserType)
            {
                case CheckSecurityUserTypes.NoWorking:
                    tBoxCheckSecUser.Text = "С диалогом  никто не работает";
                    tBoxCheckSecUser.Foreground = new SolidColorBrush(Colors.Black);
                    tBoxSecUserName.Text = "";
                    tBoxSecUserName.Foreground = new SolidColorBrush(Colors.Black);
                    tBoxMessageContent.IsEnabled = false;
                    btnSendMessage.IsEnabled = false;
                    break;
                case CheckSecurityUserTypes.IWorking:
                    tBoxCheckSecUser.Text = "Вы работаете с данным диалогом";
                    tBoxCheckSecUser.Foreground = new SolidColorBrush(Colors.Green);
                    tBoxSecUserName.Text = "";
                    tBoxSecUserName.Foreground = new SolidColorBrush(Colors.Green);
                    tBoxMessageContent.IsEnabled = true;
                    btnSendMessage.IsEnabled = true;
                    break;
                case CheckSecurityUserTypes.OtherWorking:
                    tBoxCheckSecUser.Text = "С диалогом работает пользователь:";
                    tBoxCheckSecUser.Foreground = new SolidColorBrush(Color.FromRgb(0xE9, 0x7C, 0x7C));
                    tBoxSecUserName.Text = _SecUserName;
                    tBoxSecUserName.Foreground = new SolidColorBrush(Color.FromRgb(0xE9, 0x7C, 0x7C));
                    tBoxMessageContent.IsEnabled = false;
                    btnSendMessage.IsEnabled = false;
                    break;
            }
        }
        //-----------------------------------------------------------------------------------
        private void btnSetBackgroundPanelDialogWPF_Click(object sender, RoutedEventArgs e)
        {
            m_SetBackgroundPanelDialogsWPF.Invoke(MessengerDialogId);
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
        private void btnCheckSecurityUser_Click(object sender, RoutedEventArgs e)
        {
            CheckSecUser _CheckSecUser = m_CheckSecUser.Invoke(MessengerDialogId, false);

            SetCheckSecurityUser(_CheckSecUser.CheckSecurityUserType, _CheckSecUser.SecUserName);
        }
        //------------------------------------------------------------------
        private void tBoxMessageContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSendMessage_Click(btnSendMessage, null);
            }
        }
        //------------------------------------------------------------------
    }
}
