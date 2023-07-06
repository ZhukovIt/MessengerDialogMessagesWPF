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
        private Action m_btnClose_Click;
        private Action m_btnExpandWindow_Click;
        private Action<int> m_MessengerDialogGrid_MouseDown;
        //------------------------------------------------------------------
        public BackgroundPanelDialogsWPF()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------------  
        public void Init(IEnumerable<MessengerDialogForBackgroundPanel> _MessengerDialogs,
            Tuple<Action, Action, Action<int>> _Handlers)
        {
            m_btnClose_Click = _Handlers.Item1;
            m_btnExpandWindow_Click = _Handlers.Item2;
            m_MessengerDialogGrid_MouseDown = _Handlers.Item3;

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
        #region Обработчики событий
        //------------------------------------------------------------------
        private void MessengerDialogGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Grid _Sender = (Grid)sender;

                int _MessengerDialogId = Convert.ToInt32(_Sender.Name.Split('_')[1]);

                m_MessengerDialogGrid_MouseDown.Invoke(_MessengerDialogId);
            }
        }
        //------------------------------------------------------------------
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            m_btnClose_Click.Invoke();
        }
        //------------------------------------------------------------------
        private void btnExpandWindow_Click(object sender, RoutedEventArgs e)
        {
            m_btnExpandWindow_Click.Invoke();
        }
        //------------------------------------------------------------------
        #endregion
        //------------------------------------------------------------------
    }
}
