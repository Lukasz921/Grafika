using Grafika.Elements;
using Grafika.ExtraCommands;
using System.Drawing.Drawing2D;

namespace Grafika.Controls
{
    internal class Polygon : Panel
    {
        public bool IsDragging { get; set; } = false;
        public bool IsCtrlClicked { get; set; } = false;
        public Point MouseOffset { get; set; } = Point.Empty;
        public List<Vertex> Vertices { get; set; } = [];
        public List<Edge> Edges { get; set; } = [];
        public List<(BezierSegment, BezierSegment, Edge)> Segments { get; set; } = [];
        public MoveCommands MCommands { get; set; }
        public ToggleCommands TCommands { get; set; }
        public EdgeRightClickMenu ERightClickMenu { get; set; }
        public Polygon()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            MCommands = new(this);
            TCommands = new(this);
            ERightClickMenu = new(this);
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
                edge.Visual.DrawEdge(g, edge, brush, pen1, this);
                string text = edge.Label.Label(edge);
                Visuals.ILabel.DrawEdgeLabel(g, edge, text);
            }
            foreach (var element in Segments)
            {
                g.DrawLine(pen2, element.Item3.V1.Center(), element.Item1.Center());
                g.DrawLine(pen2, element.Item1.Center(), element.Item2.Center());
                g.DrawLine(pen2, element.Item2.Center(), element.Item3.V2.Center());
            }
        }
    }
}