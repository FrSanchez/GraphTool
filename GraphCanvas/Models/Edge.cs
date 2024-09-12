using System.ComponentModel;

namespace GraphCanvas.Models;

public class Edge(Node start, Node end) : INotifyPropertyChanged
{
    public Node StartNode
    {
        get => start;
        set
        {
            start = value;
            OnPropertyChanged(nameof(StartNode));
        }
    }

    public Node EndNode
    {
        get => end;
        set
        {
            end = value;
            OnPropertyChanged(nameof(EndNode));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
