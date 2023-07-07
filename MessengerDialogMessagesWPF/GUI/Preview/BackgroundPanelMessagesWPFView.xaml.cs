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
    public partial class BackgroundPanelMessagesWPFView : UserControl
    {
        public BackgroundPanelMessagesWPFView()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------------------
        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Сообщение отправлено!\r\nТекст сообщения: {tBoxMessageContent.Text}");
        }
        //------------------------------------------------------------------------
        private void tBoxMessageContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSendMessage_Click(btnSendMessage, null);
            }
        }
        //------------------------------------------------------------------------
    }
}
