using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MessengerDialogMessagesWPF.Service
{
    public interface IMessengerService
    {
        void AddMessage(MessengerDialogMessage _Message, bool _HasAttachment);

        void UpdateMessage(MessengerDialogMessage _Message);

        FrameworkElement FindFrameworkElementFromKey(Panel _Parent, string _Key);
    }
}
