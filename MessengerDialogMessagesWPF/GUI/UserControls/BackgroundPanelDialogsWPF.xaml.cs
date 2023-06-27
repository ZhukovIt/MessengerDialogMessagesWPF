using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessengerDialogMessagesWPF.Factory;

namespace MessengerDialogMessagesWPF
{
    /// <summary>
    /// Логика взаимодействия для BackgroundPanelDialogsWPF.xaml
    /// </summary>
    public partial class BackgroundPanelDialogsWPF : UserControl
    {
        private AbstractWPFCreator m_Factory;
        //------------------------------------------------------------------
        public BackgroundPanelDialogsWPF()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------------
        public void Init(IEnumerable<MessengerDialogForBackgroundPanel> _MessengerDialogs)
        {
            m_Factory = new BackgroundPanelDialogsWPFFactory(Resources);

            foreach (var _MessengerDialog in _MessengerDialogs)
            {
                Grid _MessengerDialogGrid = (Grid)m_Factory.Create(new RequestInfo(
                    (uint)BackgroundPanelDialogsWPFElementTypes.MessengerDialogGrid, 
                    _MessengerDialog));

                _MessengerDialogGrid.MouseDown += MessengerDialogGrid_MouseDown;

                spDialogs.Children.Add(_MessengerDialogGrid);
                spDialogs.Children.Add(m_Factory.Create(new RequestInfo((uint)BackgroundPanelDialogsWPFElementTypes.DialogSeparatorGrid)));
            }
        }
        //------------------------------------------------------------------
        private void MessengerDialogGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Grid _Sender = (Grid)sender;

                int _MessengerDialogId = Convert.ToInt32(_Sender.Name.Split('_')[1]);

                MessageBox.Show($"Диалог №{_MessengerDialogId}");
            }
        }
        //------------------------------------------------------------------
    }
}
