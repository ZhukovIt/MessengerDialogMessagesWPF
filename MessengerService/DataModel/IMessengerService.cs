using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerService.DataModel
{
    public interface IMessengerService
    {
        void AddMessage(MessengerDialogMessage _Message, bool _HasAttachment);
    }
}
