using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MessengerDialogMessagesWPF.Service
{
    public sealed class MessengerDialogMessagesWPFService : IMessengerService
    {
        public void AddMessage(MessengerDialogMessage _Message, bool _HasAttachment)
        {
            throw new NotImplementedException();
        }
        //--------------------------------------------------------------
        public void UpdateMessage(MessengerDialogMessage _Message)
        {
            throw new NotImplementedException();
        }
        //--------------------------------------------------------------
        public FrameworkElement FindFrameworkElementFromKey(Panel _Parent, string _Key)
        {
            FrameworkElement findFrameworkElement;

            foreach (FrameworkElement element in _Parent.Children)
            {
                findFrameworkElement = HandleElementForFindFrameworkElement(element, _Key);

                if (findFrameworkElement.Name == _Key)
                {
                    return findFrameworkElement;
                }
            }

            return null;
        }
        //--------------------------------------------------------------
        public bool IsShowAttachmentFromFileType(string _FileType)
        {
            switch (_FileType)
            {
                case "jpg":
                case "jpeg":
                case "png":
                case "ico":
                case "gif":
                case "svg":
                case "bmp":
                case "tiff":
                case "photo":
                case "sticker":
                    return true;
                default:
                    return false;
            }
        }
        //--------------------------------------------------------------
        #region Детали имплементации сервиса
        //--------------------------------------------------------------
        private FrameworkElement HandleElementForFindFrameworkElement(FrameworkElement element, string _Key)
        {
            if (element is Border)
            {
                return HandleElementForFindFrameworkElement((FrameworkElement)(element as Border).Child, _Key);
            }
            else if (element is Panel)
            {
                FrameworkElement findElement = FindFrameworkElementFromKey(element as Panel, _Key);

                if (findElement != null)
                {
                    return findElement;
                }
            }

            return element;
        }
        //--------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------
    }
}
