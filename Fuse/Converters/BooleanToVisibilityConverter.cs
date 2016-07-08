using System;
using System.Windows;
using System.Windows.Data;

namespace Fuse.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object pValue, Type pTargetType, object pParam, System.Globalization.CultureInfo pCulture)
        {
            bool isVisible = (bool)pValue;

            if (IsVisibilityInverted(pParam))
            {
                isVisible = !isVisible;
            }

            return (isVisible ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object pValue, Type pTargetType, object pParam, System.Globalization.CultureInfo pCulture)
        {
            bool isVisible = (Visibility)pValue == Visibility.Visible;

            if (IsVisibilityInverted(pParam))
            {
                isVisible = !isVisible;
            }

            return isVisible;
        }

        private static Visibility GetVisibilityMode(object pParam)
        {
            Visibility mode = Visibility.Visible;

            if (pParam != null)
            {
                if (pParam is Visibility)
                {
                    mode = (Visibility)pParam;
                }
                else
                {
                    try
                    {
                        mode = (Visibility)Enum.Parse(typeof(Visibility), pParam.ToString(), true);
                    }
                    catch(FormatException e)
                    {
                        throw new FormatException("Invalid Visibility specified. Use Visible or Collapsed.", e);
                    }
                }
            }

            return mode;
        }

        private static bool IsVisibilityInverted(object pParam)
        {
            return (GetVisibilityMode(pParam) == Visibility.Collapsed);
        }
    }
}
