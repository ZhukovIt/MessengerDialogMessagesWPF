﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MessengerDialogMessagesWPF
{
    public sealed class MessengerDialogMessage
    {
        public string TextMessage { get; set; }

        public string DepartureDate { get; set; }

        public string DepartureTime { get; set; }

        public string UserName { get; set; }

        public string ClientName { get; set; }

        public byte[] ClientPhoto { get; set; }

        public string MessageSourceUserView { get; set; }

        public string MessageSourceClientView { get; set; }

        public MessageTypeWPF MessageTypeWPF { get; set; }

        public bool IsNewMessage { get; set; }

        public IEnumerable<MessengerDialogMessageAttachment> Attachments { get; set; }
    }
}