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

namespace MessengerDialogMessagesWPF
{
    /// <summary>
    /// Логика взаимодействия для BackgroundPanelMessagesWPF.xaml
    /// </summary>
    public partial class BackgroundPanelMessagesWPF : UserControl
    {
        public BackgroundPanelMessagesWPF()
        {
            InitializeComponent();
        }
        //-----------------------------------------------------------------------------------
        private void tBoxMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox _Sender = (TextBox)sender;
            double _TextWidth = MeasureTextWorker.MeasureWidthText(_Sender);
            double _MaxWidth = _Sender.Width - 50;
            if (_MaxWidth - _TextWidth <= 0)
            {

            }
        }
        //-----------------------------------------------------------------------------------
    }
}
