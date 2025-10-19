using Grafika.Controls;
using Grafika.Elements;

namespace Grafika.Visuals
{
    internal class Bezier : IVisual
    {
        public void DrawEdge(Graphics g, Edge e, SolidBrush brush, Pen pen1, Pen pen2, Polygon polygon)
        {
            g.DrawLine(pen1, e.V1.Center(), e.V2.Center());
            foreach (var element in polygon.Segments)
            {
                if (element.Item3 == e)
                {
                    if (e.V1.Type == VertexType.G0 && e.V2.Type == VertexType.G0) DrawBezier(g, e.V1.Center(), element.Item1.Center(), element.Item2.Center(), e.V2.Center(), pen1);
                    else DrawBezierG1(g, e.V1, element.Item1, element.Item2, e.V2, e, pen1);
                    break;
                }
            }
        }
        public static void DrawBezier(Graphics g, PointF p0, PointF p1, PointF p2, PointF p3, Pen pen, int segments = 64)
        {
            if (segments < 1) segments = 1;
            float dt = 1f / segments;
            PointF prev = p0;
            for (int i = 1; i <= segments; i++)
            {
                float t = i * dt;
                float u = 1 - t;
                float b0 = u * u * u;
                float b1 = 3f * u * u * t;
                float b2 = 3f * u * t * t;
                float b3 = t * t * t;
                float x = b0 * p0.X + b1 * p1.X + b2 * p2.X + b3 * p3.X;
                float y = b0 * p0.Y + b1 * p1.Y + b2 * p2.Y + b3 * p3.Y;
                PointF cur = new(x, y);
                g.DrawLine(pen, prev, cur);
                prev = cur;
            }
        }
        public static void DrawBezierG1(Graphics g, Vertex v0, BezierSegment b1, BezierSegment b2, Vertex v3, Edge edge, Pen pen, int segments = 64)
        {
            var p0 = new PointF(v0.Center().X, v0.Center().Y);
            var p1 = new PointF(b1.Center().X, b1.Center().Y);
            var p2 = new PointF(b2.Center().X, b2.Center().Y);
            var p3 = new PointF(v3.Center().X, v3.Center().Y);
            bool startG1 = v0.Type == VertexType.G1;
            bool endG1 = v3.Type == VertexType.G1;
            if (!startG1 && !endG1)
            {
                DrawBezier(g, p0, p1, p2, p3, pen, segments);
                return;
            }
            PointF newP1 = p1;
            PointF newP2 = p2;
            double distP03 = Math.Sqrt((p3.X - p0.X) * (p3.X - p0.X) + (p3.Y - p0.Y) * (p3.Y - p0.Y));
            float defaultLen = (float)(distP03 / 3.0);
            if (startG1)
            {
                var t = TryGetAdjacentTangentSimple(v0, edge);
                if (t.HasValue)
                {
                    var tv = t.Value;
                    float dot = tv.X * (p3.X - p0.X) + tv.Y * (p3.Y - p0.Y);
                    if (dot < 0) { tv = new PointF(-tv.X, -tv.Y); }

                    double curLen = Math.Sqrt((p1.X - p0.X) * (p1.X - p0.X) + (p1.Y - p0.Y) * (p1.Y - p0.Y));
                    float len = (curLen > 1e-3) ? (float)curLen : defaultLen;
                    newP1 = new PointF(p0.X + tv.X * len, p0.Y + tv.Y * len);
                }
                else
                {
                    DrawBezier(g, p0, p1, p2, p3, pen, segments);
                    return;
                }
            }

            if (endG1)
            {
                var t = TryGetAdjacentTangentSimple(v3, edge);
                if (t.HasValue)
                {
                    var tv = t.Value;
                    float dot = tv.X * (p0.X - p3.X) + tv.Y * (p0.Y - p3.Y);
                    if (dot < 0) { tv = new PointF(-tv.X, -tv.Y); }

                    double curLen = Math.Sqrt((p2.X - p3.X) * (p2.X - p3.X) + (p2.Y - p3.Y) * (p2.Y - p3.Y));
                    float len = (curLen > 1e-3) ? (float)curLen : defaultLen;
                    newP2 = new PointF(p3.X - tv.X * len, p3.Y - tv.Y * len);
                }
                else
                {
                    DrawBezier(g, p0, p1, p2, p3, pen, segments);
                    return;
                }
            }
            DrawBezier(g, p0, newP1, newP2, p3, pen, segments);
        }
        public static PointF? TryGetAdjacentTangentSimple(Vertex v, Edge edge)
        {
            Edge adj = (v.LeftEdge != null && v.LeftEdge != edge) ? v.LeftEdge : (v.RightEdge != null && v.RightEdge != edge) ? v.RightEdge : null!;
            if (adj == null) return null;
            Vertex other = (adj.V1 == v) ? adj.V2 : adj.V1;
            double dx = other.Center().X - v.Center().X;
            double dy = other.Center().Y - v.Center().Y;
            double len = Math.Sqrt(dx * dx + dy * dy);
            if (len < 1e-6) return null;
            return new PointF((float)(dx / len), (float)(dy / len));
        }
    }
}