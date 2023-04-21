using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerDialogMessagesWPF
{
    public sealed class MessengerDialogMessageAttachment
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public byte[] PriviewImage { get; set; }
    }
}
