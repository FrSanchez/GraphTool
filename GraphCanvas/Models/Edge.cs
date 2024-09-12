using System.ComponentModel;
using System.Text.Json.Serialization;

namespace GraphCanvas.Models;

public class Edge(Vertex start, Vertex end) : INotifyPropertyChanged
{
    [JsonIgnore]
    public Vertex StartVertex
    {
        get => start;
        set
        {
            start = value;
            OnPropertyChanged(nameof(StartVertex));
        }
    }

    [JsonIgnore]
    public Vertex EndVertex
    {
        get => end;
        set
        {
            end = value;
            OnPropertyChanged(nameof(EndVertex));
        }
    }
    
    public bool Contains(Vertex vertex)
    {
        return start == vertex || end == vertex;
    }
    
    public string Start { get; } = start.Name;
    public string End { get; } = end.Name;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
