using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform.Storage;
using GraphCanvas.Models;
using Newtonsoft.Json;
using ReactiveUI;

namespace GraphCanvas.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Vertex?> VertexList { get; private set; } = [];
    public ObservableCollection<Edge> EdgeList { get; private set;  } = [];
    public ReactiveCommand<Vertex, Unit> DeleteCommand { get; }
    public ReactiveCommand<Vertex, Unit> StartCommand { get;  }

    private Vertex? _selectedVertex = null;

    public MainWindowViewModel()
    {
        DeleteCommand = ReactiveCommand.Create<Vertex>(Delete);
        StartCommand = ReactiveCommand.Create<Vertex>(Start);
    }

    private void Start(Vertex vertex)
    {
        if (_selectedVertex == null && !vertex.Selected)
        {
            vertex.Selected = true;
            _selectedVertex = vertex;
        }

        if (_selectedVertex != null && !vertex.Selected)
        {
            _selectedVertex.Selected = false;
            AddEdge(_selectedVertex, vertex);
            _selectedVertex = null;
        }
    }

    private int _vertexNum = 0;
    
    public Vertex AddVertex(Point position)
    {
        var vertex = new Vertex($"{_vertexNum++}", position);
        Console.WriteLine($"Adding vertex {vertex.Name}");
        VertexList.Add(vertex);
        return vertex;
    }

    public Edge? AddEdge(Vertex? startVertex, Vertex? endVertex)
    {
        if (startVertex == null || endVertex == null  || startVertex.Name == endVertex.Name)
        {
            return null;
        }
        // order the vertices alphabetically
        if (string.Compare(startVertex.Name, endVertex.Name, StringComparison.Ordinal) > 0)
        {
            (startVertex, endVertex) = (endVertex, startVertex);
        }

        var edge = new Edge(startVertex, endVertex);
        if (EdgeList.Remove(edge)) return null;
        EdgeList.Add(edge);
        return edge;

    }
    
    public void Delete(Vertex vertex)
    {
        var tbd = EdgeList.Where(edge => edge.Contains(vertex)).ToList();

        foreach (var edge in tbd)
        {
            EdgeList.Remove(edge);
        }

        if (VertexList.Remove(vertex))
        {
            if (int.TryParse(VertexList.Last()?.Name, out var num))
            {
                _vertexNum = num + 1;
            }
        }
        Console.Error.WriteLine("Delete vertex failed {vertex.Name}");
    }

    public async Task Save(IStorageFile file)
    {
        // Open writing stream from the file.
        await using var stream = await file.OpenWriteAsync();
        await using var streamWriter = new StreamWriter(stream);
        var edges = EdgeList.Select(edge => new KeyValuePair<string?, string>(edge.StartVertex.Name, edge.EndVertex.Name)).ToList();
        var graph = new GraphModel() { Vertices = VertexList, Edges = EdgeList, VertexNum = _vertexNum };
        var graphSerialized = JsonConvert.SerializeObject(graph);
        // Write some content to the file.∫
        await streamWriter.WriteLineAsync(graphSerialized);

    }

    public async Task Load(IStorageFile file)
    {
        // Open reading stream from the first file.
        await using var stream = await file.OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        // Reads all the content of file as a text.
        var fileContent = await streamReader.ReadToEndAsync();
        var graph = JsonConvert.DeserializeObject<GraphModel>(fileContent);
        New();
        if (graph != null && graph.Vertices != null)
        {
            var dict = new Dictionary<string, Vertex?>();
            foreach (var vtx in graph.Vertices)
            {
                if (vtx?.Name != null) dict.Add(vtx.Name, vtx);
                VertexList.Add(vtx);
            }
            _vertexNum = graph.VertexNum;
            foreach(var edge in graph.Edges)
            {
                Console.WriteLine($"Creating edge {edge}");
                if (edge is { End: not null, Start: not null } && dict.TryGetValue(edge.Start, out var start) && dict.TryGetValue(edge.End, out var end))
                {
                    EdgeList.Add(new Edge(start, end));
                }
                else
                {
                    throw new ApplicationException("invalid edge data in file");
                }
            }
        }
    }

    public void New()
    {
        VertexList.Clear();
        EdgeList.Clear();
        _vertexNum = 0;
    }
}