using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace GraphCanvas.Infrastructure;

public class BoolToColorConverter : IValueConverter
{
    public static readonly BoolToColorConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool flag)
        {
            return flag ? new SolidColorBrush(Colors.Khaki) : new SolidColorBrush(Colors.Gray);
        }

        throw new ArgumentException();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}