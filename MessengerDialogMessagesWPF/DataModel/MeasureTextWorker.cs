using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MessengerDialogMessagesWPF
{
    public static class MeasureTextWorker
    {
        public static double MeasureWidthText(string _FontFamily, double _FontSize, string _Text)
        {
            var formattedText =
                new FormattedText(
                    _Text,
                    CultureInfo.CurrentCulture,
                    System.Windows.FlowDirection.LeftToRight,
                    new Typeface(
                        new System.Windows.Media.FontFamily(_FontFamily),
                        new System.Windows.FontStyle(),
                        FontWeights.Normal,
                        FontStretches.Normal),
                    _FontSize,
                    System.Windows.Media.Brushes.Black);
            var geometry = formattedText.BuildGeometry(new System.Windows.Point());
            var bounds = geometry.Bounds;
            return bounds.Width;
        }
        //-----------------------------------------------------------------------------------------------
        public static double MeasureWidthText(TextBox _TextBox)
        {
            return MeasureWidthText(_TextBox.FontFamily.Source, _TextBox.FontSize, _TextBox.Text);
        }
        //-----------------------------------------------------------------------------------------------
    }
}
