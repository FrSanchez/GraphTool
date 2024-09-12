using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace GraphCanvas.Infrastructure;

public class PositionConverter : IValueConverter
{
    public static PositionConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            double d => d - 15 ,
            int d => d - 15,
            _ => throw new ArgumentException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}