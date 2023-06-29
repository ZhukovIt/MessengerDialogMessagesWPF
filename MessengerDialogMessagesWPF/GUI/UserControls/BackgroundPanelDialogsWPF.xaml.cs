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
        private Point m_LastLocation;
        //------------------------------------------------------------------
        public BackgroundPanelDialogsWPF()
        {
            InitializeComponent();

            MemoryStream stream1 = null;
            MemoryStream stream2 = null;
            byte[] _ClientPhoto;
            byte[] _MessengerIcon;

            try
            {
                stream1 = new MemoryStream();
                stream1.Position = 0;
                var _ClientImage = System.Drawing.Image.FromFile(@"C:\Users\SM11\Desktop\WPF Messages\MessengerDialogMessagesWPF\MessengerDialogMessagesWPF\Resources\businessman48.png");
                _ClientImage.Save(stream1, System.Drawing.Imaging.ImageFormat.Png);
                _ClientPhoto = stream1.ToArray();

                stream2 = new MemoryStream();
                stream2.Position = 0;
                var _MessengerImage = System.Drawing.Image.FromFile(@"C:\Users\SM11\Desktop\WPF Messages\MessengerDialogMessagesWPF\MessengerDialogMessagesWPF\Resources\vk.png");
                _MessengerImage.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
                _MessengerIcon = stream2.ToArray();
            }
            finally
            {
                stream1.Dispose();
                stream2.Dispose();
            }

            List<MessengerDialogForBackgroundPanel> _MessengerDialogs = new List<MessengerDialogForBackgroundPanel>()
            {
                new MessengerDialogForBackgroundPanel()
                {
                    ClientName = "Пациент№1",
                    CountMessages = "5",
                    ClientPhoto = _ClientPhoto,
                    DialogDateTime = "06:12 29.06.2023",
                    LastMessageText = "Тест1",
                    MessengerDialogId = 1,
                    MessengerImage = _MessengerIcon
                },

                new MessengerDialogForBackgroundPanel()
                {
                    ClientName = "Пациент№2",
                    CountMessages = "3",
                    ClientPhoto = _ClientPhoto,
                    DialogDateTime = "07:33 29.06.2023",
                    LastMessageText = "Тест2",
                    MessengerDialogId = 2,
                    MessengerImage = _MessengerIcon
                },

                new MessengerDialogForBackgroundPanel()
                {
                    ClientName = "Пациент№3",
                    CountMessages = "25",
                    ClientPhoto = _ClientPhoto,
                    DialogDateTime = "07:55 29.06.2023",
                    LastMessageText = "Тест3",
                    MessengerDialogId = 3,
                    MessengerImage = _MessengerIcon
                }
            };

            Init(_MessengerDialogs, null);
        }
        //------------------------------------------------------------------  
        public void Init(IEnumerable<MessengerDialogForBackgroundPanel> _MessengerDialogs,
            Tuple<Action> _Handlers)
        {
            //m_btnClose_Click = _Handlers.Item1;

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

                MessageBox.Show($"Диалог №{_MessengerDialogId}");
            }
        }
        //------------------------------------------------------------------
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            m_btnClose_Click.Invoke();
        }
        //------------------------------------------------------------------
        #endregion
        //------------------------------------------------------------------
    }
}
