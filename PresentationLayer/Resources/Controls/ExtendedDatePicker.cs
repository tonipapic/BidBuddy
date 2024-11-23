using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PresentationLayer.Resources.Controls
{
    public class ExtendedDatePicker : DatePicker
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DatePickerTextBox box = GetTextBox();
            box.ApplyTemplate();
            ContentControl watermark = GetWatermarkControl(box);
            watermark.Content = "Unesite datum";
        }

        private DatePickerTextBox GetTextBox()
        {
            return GetTemplateChild("PART_TextBox") as DatePickerTextBox;
        }

        private static ContentControl GetWatermarkControl(DatePickerTextBox box)
        {
            return box.Template.FindName("PART_Watermark", box) as ContentControl;
        }
    }
}
