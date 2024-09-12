using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform.Storage;
using GraphCanvas.Models;

namespace GraphCanvas.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Vertex> VertexList { get; private set; } = [];
    public ObservableCollection<Edge> EdgeList { get; private set;  } = [];

    private int _vertexNum = 0;
    
    public void AddVertex(Point position)
    {
        var vertex = new Vertex($"{_vertexNum++}", position);
        Console.WriteLine($"Adding vertex {vertex.Name}");
        VertexList.Add(vertex);
    }

    public void AddEdge(Vertex startVertex, Vertex endVertex)
    {
        if (startVertex.Name == endVertex.Name)
        {
            return;
        }
        // order the vertices alphabetically
        if (string.Compare(startVertex.Name, endVertex.Name, StringComparison.Ordinal) > 0)
        {
            (startVertex, endVertex) = (endVertex, startVertex);
        }

        var edge = new Edge(startVertex, endVertex);
        if (!EdgeList.Contains(edge))
        {
            EdgeList.Add(edge);
        }
    }
    
    public void Delete(Vertex vertex)
    {
        var tbd = EdgeList.Where(edge => edge.Contains(vertex)).ToList();

        foreach (var edge in tbd)
        {
            EdgeList.Remove(edge);
        }
        VertexList.Remove(vertex);
    }

    public async Task Save(IStorageFile file)
    {
        // Open writing stream from the file.
        await using var stream = await file.OpenWriteAsync();
        await using var streamWriter = new StreamWriter(stream);
        var edges = EdgeList.Select(edge => new KeyValuePair<string, string>(edge.StartVertex.Name, edge.EndVertex.Name)).ToList();
        var graph = new GraphModel() { Vertices = VertexList, Edges = EdgeList, VertexNum = _vertexNum };
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
        VertexList = new ObservableCollection<Vertex> (graph.Vertices);
        _vertexNum = graph.VertexNum;
    }

    public void New()
    {
        VertexList.Clear();
        EdgeList.Clear();
        _vertexNum = 0;
    }
}