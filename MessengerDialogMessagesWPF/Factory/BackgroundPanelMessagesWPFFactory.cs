using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MessengerDialogMessagesWPF.Factory
{
    public sealed class BackgroundPanelMessagesWPFFactory : AbstractWPFCreator
    {
        private BackgroundPanelMessagesWPF m_MainControl;
        //-----------------------------------------------------------------------------------
        public BackgroundPanelMessagesWPFFactory(BackgroundPanelMessagesWPF _MainControl, ResourceDictionary _Resources) : base(_Resources)
        {
            m_MainControl = _MainControl;
        }
        //-----------------------------------------------------------------------------------
        public override FrameworkElement Create(RequestInfo _RequestInfo)
        {
            switch ((BackgroundPanelMessagesWPFElementTypes)_RequestInfo.ElementType)
            {
                case BackgroundPanelMessagesWPFElementTypes.BorderDepartureDate:
                    return CreateBorderDepartureDate();
                default:
                    throw new NotImplementedException(
                        $"Элемент типа: {(BackgroundPanelDialogsWPFElementTypes)_RequestInfo.ElementType} не поддерживается данной фабрикой!");
            }
        }
        //-----------------------------------------------------------------------------------
        public Border CreateBorderDepartureDate()
        {
            return new Border();
        }
        //-----------------------------------------------------------------------------------
    }
}
