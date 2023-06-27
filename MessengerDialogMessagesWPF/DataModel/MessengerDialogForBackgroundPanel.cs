using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerDialogMessagesWPF
{
    public sealed class MessengerDialogForBackgroundPanel
    {
        public int MessengerDialogId { get; set; }

        public byte[] ClientPhoto { get; set; }

        public string ClientName { get; set; }

        public string DialogDateTime { get; set; }

        public byte[] MessengerImage { get; set; }

        public string CountMessages { get; set; }

        public string LastMessageText { get; set; }
    }
}
