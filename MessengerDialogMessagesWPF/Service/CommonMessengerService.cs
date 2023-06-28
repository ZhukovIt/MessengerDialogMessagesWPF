using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MessengerDialogMessagesWPF.Service
{
    public abstract class CommonMessengerService
    {
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
        public bool HasNameFrameworkElementInElements(IEnumerable _Elements, string _Name)
        {
            foreach (FrameworkElement _Element in _Elements)
            {
                if (_Element.Name == _Name)
                {
                    return true;
                }
            }

            return false;
        }
        //--------------------------------------------------------------
        public virtual void AddMessage(Panel Container, MessengerDialogMessage _Message, bool _HasAttachment)
        {
            throw new NotImplementedException("Метод AddMessage в классе CommonMessengerService не поддерживается!");
        }
        //--------------------------------------------------------------
        public virtual void UpdateMessage(Panel Container, MessengerDialogMessage _Message)
        {
            throw new NotImplementedException("Метод UpdateMessage в классе CommonMessengerService не поддерживается!");
        }
        //--------------------------------------------------------------
        public virtual void Clear(Panel Container)
        {
            throw new NotImplementedException("Метод Clear в классе CommonMessengerService не поддерживается!");
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
