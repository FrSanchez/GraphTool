using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform.Storage;
using GraphCanvas.Models;
using Node = GraphCanvas.Models.Node;

namespace GraphCanvas.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Node> NodeList { get; private set; } = [];
    public ObservableCollection<Edge> EdgeList { get; private set;  } = [];

    private int _nodeNum = 0;
    
    public void AddNode(Point position)
    {
        // Create a new node at the clicked position
        var newNode = new Node($"{_nodeNum++}", position);
        Console.WriteLine($"Adding node {newNode.Name}");
        NodeList.Add(newNode);
    }

    public void AddEdge(Node startNode, Node endNode)
    {
        if (startNode.Name == endNode.Name)
        {
            return;
        }
        // order the nodes alphabetically
        if (string.Compare(startNode.Name, endNode.Name, StringComparison.Ordinal) > 0)
        {
            (startNode, endNode) = (endNode, startNode);
        }

        var edge = new Edge(startNode, endNode);
        if (!EdgeList.Contains(edge))
        {
            EdgeList.Add(edge);
        }
    }
    
    public void Delete(Node draggedNode)
    {
        NodeList.Remove(draggedNode);
    }

    public async Task Save(IStorageFile file)
    {
        // Open writing stream from the file.
        await using var stream = await file.OpenWriteAsync();
        await using var streamWriter = new StreamWriter(stream);
        // Serialize the NodeList to JSON.
        var graph = new GraphModel() { Nodes = NodeList, Edges = EdgeList, NodeNum = _nodeNum };
        var graphSerialized = JsonSerializer.Serialize(graph);
        // Write some content to the file.
        await streamWriter.WriteLineAsync(graphSerialized);

    }

    public async Task Load(IStorageFile file)
    {
        // Open reading stream from the first file.
        await using var stream = await file.OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        // Reads all the content of file as a text.
        var fileContent = await streamReader.ReadToEndAsync();
        var graph = JsonSerializer.Deserialize<GraphModel>(fileContent);
        NodeList = new ObservableCollection<Node> (graph.Nodes);
        EdgeList = new ObservableCollection<Edge>(graph.Edges);
        _nodeNum = graph.NodeNum;
    }
}