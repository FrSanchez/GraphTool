using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Point = Avalonia.Point;

namespace GraphCanvas.Models;

public class Vertex : INotifyPropertyChanged, IComparable<Vertex>
{
    private string? _name;
    private Point _position;
    private bool _selected;

    public Vertex(string? name, Point position)
    {
        _name = name;
        _position = position;
        _selected = false;
    }

    [JsonIgnore]
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            OnPropertyChanged(nameof(Selected));
        }
    }
    public string? Name
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
            _position = value;
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