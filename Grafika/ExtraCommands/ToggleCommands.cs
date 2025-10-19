using Grafika.Controls;
using Grafika.Elements;

namespace Grafika.ExtraCommands
{
    internal class ToggleCommands(Polygon polygon)
    {
        public Polygon Polygon { get; set; } = polygon;
        public void AddVertexOnMiddle(Edge e)
        {
            int newX = (e.V1.Center().X + e.V2.Center().X) / 2;
            int newY = (e.V1.Center().Y + e.V2.Center().Y) / 2;

            Vertex v = new(new(newX, newY));
            Edge e1 = new(e.V1, v);
            Edge e2 = new(e.V2, v);

            if (e.V1.LeftEdge != null && e.V1.LeftEdge == e)
            {
                v.RightEdge = e1;
                v.LeftEdge = e2;
                e.V1.LeftEdge = e1;
                e.V2.RightEdge = e2;
            }
            else
            {
                v.LeftEdge = e1;
                v.RightEdge = e2;
                e.V1.RightEdge = e1;
                e.V2.LeftEdge = e2;
            }

            Polygon.Vertices.Add(v);
            Polygon.Controls.Add(v);
            Polygon.Edges.Remove(e);
            Polygon.Edges.Add(e1);
            Polygon.Edges.Add(e2);
        }
        public void RemoveVertexOnMiddle(Vertex v)
        {
            if (Polygon.Vertices.Count <= 3) return;

            for (int i = 0; i < Polygon.Segments.Count; i++)
            {
                if (Polygon.Segments[i].Item3 == v.LeftEdge)
                {
                    RemoveBesierSegment(v.LeftEdge);
                }
                if (Polygon.Segments[i].Item3 == v.RightEdge)
                {
                    RemoveBesierSegment(v.RightEdge);
                }
            }

            List<Edge> edges = [];
            foreach (Edge edge in Polygon.Edges)
            {
                if (edge.V1 == v || edge.V2 == v) edges.Add(edge);
            }
            Vertex v1;
            if (edges[0].V2 == v) v1 = edges[0].V1;
            else v1 = edges[0].V2;
            Vertex v2;
            if (edges[1].V2 == v) v2 = edges[1].V1;
            else v2 = edges[1].V2;

            Edge e = new(v1, v2);

            if (v1.LeftEdge != null && (v1.LeftEdge.V1 == v || v1.LeftEdge.V2 == v))
            {
                v1.LeftEdge = e;
                v2.RightEdge = e;
            }
            else
            {
                v1.RightEdge = e;
                v2.LeftEdge = e;
            }

            Polygon.Vertices.Remove(v);
            Polygon.Controls.Remove(v);
            Polygon.Edges.Add(e);
            Polygon.Edges.Remove(edges[0]);
            Polygon.Edges.Remove(edges[1]);
        }
        public void AddBezierSegment(Edge e)
        {
            int x1 = (int)(e.V1.Center().X + (e.V2.Center().X - e.V1.Center().X) * (1.0 / 3));
            int y1 = (int)(e.V1.Center().Y + (e.V2.Center().Y - e.V1.Center().Y) * (1.0 / 3));
            int x2 = (int)(e.V1.Center().X + (e.V2.Center().X - e.V1.Center().X) * (2.0 / 3));
            int y2 = (int)(e.V1.Center().Y + (e.V2.Center().Y - e.V1.Center().Y) * (2.0 / 3));

            BezierSegment v1 = new(new(x1, y1));
            BezierSegment v2 = new(new(x2, y2));

            v1.LeftVertex = e.V1;
            v2.RightVertex = e.V2;

            Polygon.Segments.Add((v1, v2, e));
            Polygon.Controls.Add(v1);
            Polygon.Controls.Add(v2);

            v1.BackColor = Polygon.BackColor;
            v2.BackColor = Polygon.BackColor;
            v1.BringToFront();
            v2.BringToFront();
        }
        public void RemoveBesierSegment(Edge e)
        {
            for (int i = Polygon.Segments.Count - 1; i >= 0; i--)
            {
                var element = Polygon.Segments[i];
                if (element.Item3 == e)
                {
                    Polygon.Controls.Remove(element.Item1);
                    Polygon.Controls.Remove(element.Item2);
                    Polygon.Segments.RemoveAt(i);
                    break;
                }
            }
        }
    }
}