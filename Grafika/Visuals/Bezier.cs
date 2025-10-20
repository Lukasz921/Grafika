using Grafika.Controls;
using Grafika.Elements;

namespace Grafika.Visuals
{
    internal class Bezier : IVisual
    {
        public void DrawEdge(Graphics g, Edge e, SolidBrush brush, Pen pen1, Polygon polygon)
        {
            Pen dashedPen = new(Color.Black)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            g.DrawLine(dashedPen, e.V1.Center(), e.V2.Center());
            foreach (var element in polygon.Segments)
            {
                if (element.Item3 == e)
                {
                    DrawBezier(g, e.V1.Center(), element.Item1.Center(), element.Item2.Center(), e.V2.Center(), pen1);
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
    }
}