using System;
using System.Globalization;
using System.Windows.Data;

namespace FuseControlSystem.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object pValue, Type pTargetType, object pParam, CultureInfo pCulture)
        {
            if (pTargetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean.");

            return !(bool)pValue;
        }

        public object ConvertBack(object pValue, Type pTargetType, object pParam, CultureInfo pCulture)
        {
            throw new NotSupportedException();
        }
    }
}
