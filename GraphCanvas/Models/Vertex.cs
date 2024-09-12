using System;
using System.ComponentModel;
using Avalonia;

namespace GraphCanvas.Models;

public class Vertex(string name, Point position) : INotifyPropertyChanged, IComparable<Vertex>
{
    private string _name = name;
    private Point _position = AdjustPosition(position);
    private bool _selected = false;
    private IComparable<Vertex> _comparableImplementation;

    private static Point AdjustPosition(Point position) => new Point(position.X - 10, position.Y - 10);

    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            OnPropertyChanged(nameof(Selected));
        }
    }
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

    public int CompareTo(Vertex? other)
    {
        return string.Compare(this.Name, other?.Name, StringComparison.Ordinal);
    }
}