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

namespace MessengerDialogMessagesWPF
{
    /// <summary>
    /// Логика взаимодействия для BackgroundPanelMessagesWPF.xaml
    /// </summary>
    public partial class BackgroundPanelMessagesWPF : UserControl
    {
        private AbstractWPFCreator m_Factory;
        //-----------------------------------------------------------------------------------
        public BackgroundPanelMessagesWPF()
        {
            InitializeComponent();
        }
        //-----------------------------------------------------------------------------------
        public void Init(IEnumerable<MessengerDialogMessage> _Messages)
        {
            m_Factory = new BackgroundPanelMessagesWPFFactory(this, Resources);
        }
        //-----------------------------------------------------------------------------------
    }
}
