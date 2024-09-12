using System.ComponentModel;
using Avalonia;

namespace GraphCanvas.Models;

public class Node(string name, Point position) : INotifyPropertyChanged
{
    private string _name = name;
    private Point _position = AdjustPosition(position);
    
    private static Point AdjustPosition(Point position) => new Point(position.X - 10, position.Y - 10);

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public Point Position
    {
        get => _position;
        set
        {
            _position = AdjustPosition(value);
            OnPropertyChanged(nameof(Position));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}