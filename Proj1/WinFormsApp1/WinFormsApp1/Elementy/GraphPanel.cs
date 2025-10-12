namespace WinFormsApp1.Elementy
{
    public class GraphPanel : Panel
    {
        public List<Vertex> Vertices { get; set; } = [];
        public List<Edge> Edges { get; set; } = [];
        public Edge? SelectedEdge { get; set; } = null;
        public GraphPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            UpdateStyles();
            BackColor = Color.White;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            foreach (var edge in Edges)
            {
                Point p1 = edge.A.Center;
                Point p2 = edge.B.Center;
                using var brush = new SolidBrush(Color.Black);
                if (edge == SelectedEdge) brush.Color = Color.Blue;
                if (edge.Type == EdgeType.Normal) BresenhamLine(g, p1.X, p1.Y, p2.X, p2.Y, brush);
                if (edge.Type == EdgeType.SemiCircle) BresenhamSemicircle(g, p1.X, p1.Y, p2.X, p2.Y, brush);
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
        private static void BresenhamSemicircle(Graphics g, int x0, int y0, int x1, int y1, Brush brush)
        {
            int cx = (x0 + x1) / 2;
            int cy = (y0 + y1) / 2;
            double vx = x0 - cx;
            double vy = y0 - cy;
            int R = (int)Math.Round(Math.Sqrt(vx * vx + vy * vy));
            if (R <= 0)
            {
                g.FillRectangle(brush, x0, y0, 1, 1);
                g.FillRectangle(brush, x1, y1, 1, 1);
                return;
            }
            int nx = -(y1 - y0);
            int ny = (x1 - x0);
            bool SideOk(int px, int py)
            {
                long qx = px - cx;
                long qy = py - cy;
                long side = qx * nx + qy * ny;
                return side >= 0;
            }
            void PlotIfSide(int sx, int sy)
            {
                int px = cx + sx;
                int py = cy + sy;
                if (SideOk(px, py)) g.FillRectangle(brush, px, py, 1, 1);
            }
            int x = 0;
            int y = R;
            int d = 1 - R;
            int deltaE = 3;
            int deltaSE = 5 - 2 * R;
            PlotIfSide(x, y);
            PlotIfSide(x, -y);
            PlotIfSide(-x, y);
            PlotIfSide(-x, -y);
            PlotIfSide(y, x);
            PlotIfSide(y, -x);
            PlotIfSide(-y, x);
            PlotIfSide(-y, -x);
            while (y > x)
            {
                if (d < 0)
                {
                    d += deltaE;
                    deltaE += 2;
                    deltaSE += 2;
                }
                else
                {
                    d += deltaSE;
                    deltaE += 2;
                    deltaSE += 4;
                    y--;
                }
                x++;
                PlotIfSide(x, y);
                PlotIfSide(x, -y);
                PlotIfSide(-x, y);
                PlotIfSide(-x, -y);
                PlotIfSide(y, x);
                PlotIfSide(y, -x);
                PlotIfSide(-y, x);
                PlotIfSide(-y, -x);
            }
        }
        public void AddVertex(Vertex v)
        {
            if (v == null) return;
            Vertices.Add(v);
            Controls.Add(v);
        }
        public void AddVertexOnEdge(Edge edge)
        {
            if (edge == null) return;
            var aC = edge.A.Center;
            var bC = edge.B.Center;
            int midX = (aC.X + bC.X) / 2;
            int midY = (aC.Y + bC.Y) / 2;
            var newV = new Vertex();
            newV.Location = new Point(midX - newV.Width / 2, midY - newV.Height / 2);
            AddVertex(newV);
            RemoveEdge(edge);
            AddEdge(edge.A, newV);
            AddEdge(newV, edge.B);
            if (Edges.Count > 0) SelectedEdge = Edges[0];
            else SelectedEdge = null;
            Invalidate();
        }
        public void RemoveVertex(Vertex v)
        {
            if (v == null || Vertices.Count <= 3) return;
            var incidentWithIndex = Edges.Select((edge, idx) => new { edge, idx }).Where(x => x.edge.Connects(v)).ToList();
            var incidentEdges = incidentWithIndex.Select(x => x.edge).ToList();
            var incidentIndices = incidentWithIndex.Select(x => x.idx).OrderBy(i => i).ToList();
            int insertIndex = incidentIndices.First();
            foreach (var idx in incidentIndices.OrderByDescending(i => i)) Edges.RemoveAt(idx);
            var neighbors = incidentEdges.Select(e => e.Other(v)).Distinct().ToList();
            if (neighbors.Count == 2)
            {
                var a = neighbors[0];
                var b = neighbors[1];
                bool already = Edges.Any(e => (e.A == a && e.B == b) || (e.A == b && e.B == a));
                if (!already)
                {
                    int idxToInsert = Math.Max(0, Math.Min(insertIndex, Edges.Count));
                    Edges.Insert(idxToInsert, new Edge(a, b));
                }
            }
            Vertices.Remove(v);
            if (FindForm() is Form1 form)
            {
                form.NUD1.Maximum = Math.Max(0, Edges.Count - 1);
                if (form.NUD1.Value > form.NUD1.Maximum) form.NUD1.Value = form.NUD1.Maximum;
                SelectedEdge = Edges[(int)form.NUD1.Value];
                form.NUD2.Value = (int)SelectedEdge.Length();
            }
            if (Controls.Contains(v)) Controls.Remove(v);
            v.Dispose();
            Invalidate();
        }
        public void AddEdge(Vertex a, Vertex b)
        {
            if (a == null || b == null) return;
            bool exists = Edges.Any(e => (e.A == a && e.B == b) || (e.A == b && e.B == a));
            if (!exists)
            {
                Edges.Add(new Edge(a, b));
                Invalidate();
            }
        }
        public void RemoveEdge(Edge edge)
        {
            if (edge == null) return;
            bool removed = Edges.Remove(edge);
            if (!removed) return;
            if (SelectedEdge == edge)
            {
                SelectedEdge = Edges.Count > 0 ? Edges[0] : null;
            }
            Invalidate();
        }
    }
}