using System.Text.Json;
using Avalonia;
using GraphCanvas.Models;
using Newtonsoft.Json;

namespace GraphTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestDeserialization()
    {
        var fileContent = File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "graph.json"));
        var graph = JsonConvert.DeserializeObject<GraphModel>(fileContent);
        Assert.That(graph, Is.Not.Null);
        Assert.That(graph.VertexNum, Is.EqualTo(5));
        Assert.That(graph.Vertices.Count(), Is.EqualTo(5));
        var vertex = graph.Vertices.First();
        if (vertex.Name != null) Assert.That(vertex.Name, Is.EqualTo("0"));
        Assert.That(vertex.Position.X, Is.EqualTo(385.7).Within(0.0001));
    }

    [Test]
    public void TestDeserializationEmpty()
    {
        var input = "{\"Vertices\":[],\"Edges\":[],\"VertexNum\":0}";
        var graph = JsonConvert.DeserializeObject<GraphModel>(input);
        Assert.That(graph, Is.Not.Null);
        Assert.That(graph.VertexNum, Is.EqualTo(0));
        Assert.That(graph.Vertices.Count(), Is.EqualTo(0));
        Assert.That(graph.Edges.Count(), Is.EqualTo(0));
    }

    [Test]
    public void TestSerializationEmpty()
    {
        var graph = new GraphModel()
        {
            VertexNum = 0,
            Vertices = [],
            Edges = []
        };
        var output = JsonConvert.SerializeObject(graph);
        Assert.That(output, Is.EqualTo("{\"Vertices\":[],\"Edges\":[],\"VertexNum\":0}"));
    }
}