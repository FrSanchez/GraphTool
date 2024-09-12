using Avalonia;
using GraphCanvas.ViewModels;

namespace GraphTest;

public class MainWindowsViewModelTest
{
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestNew()
    {
        var viewModel = new MainWindowViewModel();
        viewModel.New();
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel.VertexList, Is.Not.Null);
        Assert.That(viewModel.EdgeList, Is.Not.Null);
        Assert.That(viewModel.VertexList.Count(), Is.EqualTo(0));
        Assert.That(viewModel.EdgeList.Count(), Is.EqualTo(0));    
    }

    [Test]
    public void AddEdgeNull()
    {
        var viewModel = new MainWindowViewModel();
        var v0 =  viewModel.AddVertex(new Point(10,10));
        var edge = viewModel.AddEdge(v0, null);
        Assert.That(edge, Is.Null);
        edge = viewModel.AddEdge(null, v0);
        Assert.That(edge, Is.Null);
    }

    [Test]
    public void AddEdgeSame()
    {
        var viewModel = new MainWindowViewModel();
        var v0 =  viewModel.AddVertex(new Point(10,10));
        var edge = viewModel.AddEdge(v0, v0);
        Assert.That(edge, Is.Null);
    }

    [Test]
    public void AddEdgeDuplicate()
    {
        var viewModel = new MainWindowViewModel();
        var v0 =  viewModel.AddVertex(new Point(10,10));
        var v1 = viewModel.AddVertex(new Point(10, 30));
        var edge = viewModel.AddEdge(v1, v0);
        Assert.That(edge, Is.Not.Null);
        edge = viewModel.AddEdge(v1, v0);
        Assert.That(edge, Is.Null);
    }

    [Test]
    public void AddEdgeIsSorted()
    {
        var viewModel = new MainWindowViewModel();
        var v0 =  viewModel.AddVertex(new Point(10,10));
        var v1 = viewModel.AddVertex(new Point(10, 30));
        var edge = viewModel.AddEdge(v1, v0);
        Assert.That(edge, Is.Not.Null);
        Assert.That(edge.Start, Is.EqualTo(v0.Name));
        Assert.That(edge.End, Is.EqualTo(v1.Name));
    }

    [Test]
    public void AddEdge()
    {
        var viewModel = new MainWindowViewModel();
        var v0 =  viewModel.AddVertex(new Point(10,10));
        Assert.That(v0.Name, Is.EqualTo("0"));
        var v1 = viewModel.AddVertex(new Point(10,20));
        Assert.That(v1.Name, Is.EqualTo("1"));
        viewModel.AddEdge(v1, v0);
    }
}