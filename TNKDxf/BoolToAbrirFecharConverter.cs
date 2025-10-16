using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;


namespace TNKDxf
{
    public class BoolToAbrirFecharConverter : MarkupExtension, IValueConverter
    {
        private static BoolToAbrirFecharConverter converter;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Return Segoe MDL2 Assets glyphs: Cancel (E711) when true, Sync/Convert (E72E) when false
            return (value is bool && (bool)value) ? "\uE711" : "\uE72E";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not used in this scenario
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new BoolToAbrirFecharConverter());
        }
    }
}
