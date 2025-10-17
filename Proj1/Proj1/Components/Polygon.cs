using System.Drawing.Drawing2D;

namespace Proj1.Components
{
    internal partial class Polygon : Panel
    {
        public bool IsDragging { get; set; } = false;
        public bool IsCtrlClicked { get; set; } = false;
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
            foreach (Edge edge in Edges)
            {
                if (edge.Type == EdgeType.Normal)
                {
                    Visuals.BresenhamLine(g, edge, brush);
                }
                if (edge.Type == EdgeType.SemiCircle)
                {
                    if (edge.V1.Type == VertexType.G1 || edge.V2.Type == VertexType.G1) Visuals.SemiCircleG1(g, edge, pen1, pen2);
                    else Visuals.SemiCircleG0(g, edge, pen1, pen2);
                }
                if (edge.Type == EdgeType.Bezier)
                {
                    g.DrawLine(pen1, edge.V1.Center(), edge.V2.Center());
                }
                string mod = Visuals.SwitchEdgeLabel(edge.Modifier, edge.ConstLength);
                Visuals.DrawEdgeLabel(g, edge, mod);
            }
        }
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

            Vertices.Add(v);
            Controls.Add(v);
            Edges.Remove(e);
            Edges.Add(e1);
            Edges.Add(e2);
        }
        public void RemoveVertexOnMiddle(Vertex v)
        {
            if (Vertices.Count <= 3) return;

            List<Edge> edges = [];
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

            Vertices.Remove(v);
            Controls.Remove(v);
            Edges.Add(e);
            Edges.Remove(edges[0]);
            Edges.Remove(edges[1]);
        }
        public void MouseDownPolygon(Point mouse, bool isCtrlClicked, Vertex? draggedVertex)
        {
            IsDragging = true;
            IsCtrlClicked = isCtrlClicked;
            if (IsCtrlClicked) MouseOffset = mouse;
            else
            {
                if (draggedVertex != null) MouseOffset = new Point(mouse.X - draggedVertex.Location.X, mouse.Y - draggedVertex.Location.Y);
                else MouseOffset = mouse;
            }
        }
        public void MouseMovePolygon(Point mouse, Vertex dragged)
        {
            if (!IsDragging) return;
            if (IsCtrlClicked)
            {
                int dx = mouse.X - MouseOffset.X;
                int dy = mouse.Y - MouseOffset.Y;
                if (dx == 0 && dy == 0)
                {
                    MouseOffset = mouse;
                    return;
                }
                foreach (Vertex v in Vertices) v.Location = new Point(v.Location.X + dx, v.Location.Y + dy);
                MouseOffset = new Point(MouseOffset.X + dx, MouseOffset.Y + dy);
                Invalidate();
            }
            else
            {
                (List<Vertex> leftchain, List<Vertex> rightchain) = GetMoveChain(dragged);
                if (leftchain.Count == 0 && rightchain.Count == 0)
                {
                    Point newLocation = new(mouse.X - MouseOffset.X, mouse.Y - MouseOffset.Y);
                    dragged.Location = newLocation;
                    Invalidate();
                }
                else
                {
                    Point newLocation = new(mouse.X - MouseOffset.X, mouse.Y - MouseOffset.Y);
                    dragged.Location = newLocation;

                    Vertex v = dragged;
                    for (int i = 0; i < rightchain.Count; i++)
                    {
                        ApplyMove(v, rightchain[i]);
                        v = rightchain[i];
                    }
                    v = dragged;
                    for (int i = 0; i < leftchain.Count; i++)
                    {
                        ApplyMove(v, leftchain[i]);
                        v = leftchain[i];
                    }
                    Invalidate();
                }
            }
        }
        public void MouseUpPolygon()
        {
            IsDragging = false;
            IsCtrlClicked = false;
        }
    }
}