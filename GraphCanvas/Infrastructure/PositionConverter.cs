using System;
using System.Diagnostics;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace GraphCanvas.Infrastructure;

public class PositionConverter : IValueConverter
{
    public static PositionConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Console.WriteLine(value);
        Console.WriteLine(targetType);
        return value switch
        {
            double d => d - 16.0,
            Point p => p + new Point(0, 0),
            _ => throw new ArgumentException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}