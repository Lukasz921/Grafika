using Grafika.Controls;
using Grafika.Elements;

namespace Grafika.Visuals
{
    internal class Bresenham : IVisual
    {
        public void DrawEdge(Graphics g, Edge e, SolidBrush brush, Pen pen1, Polygon polygon)
        {
            int x0 = e.V1.Center().X;
            int y0 = e.V1.Center().Y;
            int x1 = e.V2.Center().X;
            int y1 = e.V2.Center().Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                g.FillRectangle(brush, x0, y0, 1, 1);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }
    }
}