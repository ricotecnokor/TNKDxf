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
            return (value is bool && (bool)value) ? "Fechar" : "Abrir";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool && (bool)value) ? "Abrir" : "Fechar";
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new BoolToAbrirFecharConverter());
        }
    }
}
