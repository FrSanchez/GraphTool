using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GraphCanvas.Models;

public class Edge(Vertex? start, Vertex? end) : INotifyPropertyChanged, IEquatable<Edge>
{
    [JsonIgnore]
    public Vertex? StartVertex
    {
        get => start;
        set
        {
            start = value;
            OnPropertyChanged(nameof(StartVertex));
        }
    }

    [JsonIgnore]
    public Vertex? EndVertex
    {
        get => end;
        set
        {
            end = value;
            OnPropertyChanged(nameof(EndVertex));
        }
    }

    // Used by json (de)serialization
    public Edge() : this(null, null)
    {
        
    }
    
    public bool Contains(Vertex vertex)
    {
        return start == vertex || end == vertex;
    }
    
    public string? Start { get; set;  } = start?.Name;
    public string? End { get; set;  } = end?.Name;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool Equals(Edge? other)
    {
        if (other == null)
            return false;
        return (Start == other.Start && End == other.End) || (Start == other.End && End == other.Start);
    }
}
