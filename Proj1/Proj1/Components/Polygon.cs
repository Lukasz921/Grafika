using System.Drawing.Drawing2D;

namespace Proj1.Components
{
    internal class Polygon : Panel
    {
        public bool IsDragging { get; set; } = false;
        public Point MouseOffset { get; set; } = Point.Empty;
        public List<Vertex> Vertices { get; set; } = [];
        public List<Edge> Edges { get; set; } = [];
        public RightClickMenu? Rcm { get; set; } = null;
        public Polygon()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            Rcm = new(this);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            SolidBrush brush = new(Color.Black);
            Pen pen1 = new(Color.Black);
            Pen pen2 = new(Color.Green);
            foreach (var edge in Edges)
            {
                if (edge.Type == EdgeType.Normal)
                {
                    BresenhamLine(g, edge.V1.Center().X, edge.V1.Center().Y, edge.V2.Center().X, edge.V2.Center().Y, brush);
                }
                if (edge.Type == EdgeType.SemiCircle)
                {
                    var a = edge.V1.Center();
                    var b = edge.V2.Center();
                    float cx = (a.X + b.X) / 2f;
                    float cy = (a.Y + b.Y) / 2f;
                    float radius = (float)(Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) / 2.0);
                    var rect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);
                    double angleRad = Math.Atan2(a.Y - cy, a.X - cx);
                    float startAngle = (float)(angleRad * 180.0 / Math.PI);
                    g.DrawArc(pen1, rect, startAngle, 180f);
                    g.DrawLine(pen2, edge.V1.Center(), edge.V2.Center());
                }
                if (edge.Type == EdgeType.Bezier)
                {
                    g.DrawLine(pen1, edge.V1.Center(), edge.V2.Center());
                }
            }
        }
        private static void BresenhamLine(Graphics g, int x0, int y0, int x1, int y1, Brush brush)
        {
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
        public void AddVertexOnMiddle(Edge e)
        {
            int newX = (e.V1.Center().X + e.V2.Center().X) / 2;
            int newY = (e.V1.Center().Y + e.V2.Center().Y) / 2;

            Vertex v = new(new(newX, newY));
            Edge e1 = new(e.V1, v);
            Edge e2 = new(e.V2, v);

            Vertices.Add(v);
            Controls.Add(v);
            Edges.Remove(e);
            Edges.Add(e1);
            Edges.Add(e2);
        }
        public void RemoveVertexOnMiddle(Vertex v)
        {
            if (Vertices.Count <= 3) return;

            List<Edge> edges = new List<Edge>();
            foreach(Edge edge in Edges)
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

            Vertices.Remove(v);
            Controls.Remove(v);
            Edges.Add(e);
            Edges.Remove(edges[0]);
            Edges.Remove(edges[1]);
        }
        public void MouseDownPolygon(Point p)
        {
            IsDragging = true;
            MouseOffset = p;
        }
        public void MouseMovePolygon(Point p)
        {
            if (!IsDragging) return;

            int dx = p.X - MouseOffset.X;
            int dy = p.Y - MouseOffset.Y;

            int minXAfter = Vertices.Min(v => v.Location.X + dx);
            int maxXAfter = Vertices.Max(v => v.Location.X + dx + v.Width);
            int minYAfter = Vertices.Min(v => v.Location.Y + dy);
            int maxYAfter = Vertices.Max(v => v.Location.Y + dy + v.Height);

            if (minXAfter < 0) dx -= minXAfter;
            if (maxXAfter > ClientSize.Width) dx -= (maxXAfter - ClientSize.Width);
            if (minYAfter < 0) dy -= minYAfter;
            if (maxYAfter > ClientSize.Height) dy -= (maxYAfter - ClientSize.Height);

            if (dx == 0 && dy == 0)
            {
                MouseOffset = p;
                return;
            }
            foreach (var v in Vertices)
            {
                v.Location = new Point(v.Location.X + dx, v.Location.Y + dy);
            }
            MouseOffset = new Point(MouseOffset.X + dx, MouseOffset.Y + dy);
            Invalidate();
        }
        public void MouseUpPolygon()
        {
            IsDragging = false;
        }
    }
}