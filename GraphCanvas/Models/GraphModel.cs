using System.Collections.Generic;

namespace GraphCanvas.Models;

public class GraphModel
{
    public IEnumerable<Node> Nodes { get; set; }
    public IEnumerable<Edge> Edges { get; set; }
    public int NodeNum { get; set; }
}