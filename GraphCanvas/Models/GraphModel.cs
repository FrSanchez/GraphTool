using System.Collections.Generic;

namespace GraphCanvas.Models;

public class GraphModel
{
    public IEnumerable<Vertex> Vertices { get; set; } = [];
    public IEnumerable<Edge> Edges { get; set; } = [];
    public int VertexNum { get; set; } = 0;
}