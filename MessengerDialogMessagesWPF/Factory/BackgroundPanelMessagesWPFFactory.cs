using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerDialogMessagesWPF.Factory
{
    public sealed class BackgroundPanelMessagesWPFFactory : AbstractWPFCreator
    {
        public BackgroundPanelMessagesWPFFactory(ResourceDictionary _Resources) : base(_Resources)
        {

        }
        //-----------------------------------------------------------------------------------
        public override FrameworkElement Create(RequestInfo _RequestInfo)
        {
            switch ((BackgroundPanelMessagesWPFElementTypes)_RequestInfo.ElementType)
            {

                default:
                    throw new NotImplementedException(
                        $"Элемент типа: {(BackgroundPanelDialogsWPFElementTypes)_RequestInfo.ElementType} не поддерживается данной фабрикой!");
            }
        }
        //-----------------------------------------------------------------------------------
    }
}
