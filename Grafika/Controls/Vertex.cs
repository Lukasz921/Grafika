using Grafika.Elements;
using Grafika.ExtraCommands;

namespace Grafika.Controls
{
    enum VertexType
    {
        G0,
        G1,
        C1,
    }
    internal class Vertex : Control
    {
        public Edge? LeftEdge { get; set; } = null;
        public Edge? RightEdge { get; set; } = null;
        public VertexType Type { get; set; } = VertexType.G0;   
        public VertexRightClickMenu VRightClickMenu { get; set; }
        public Vertex(Point p)
        {
            VRightClickMenu = new(this);
            Width = 16;
            Height = 16;
            Location = new Point(p.X - Width / 2, p.Y - Height / 2);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            Paint += Vertex_Paint;
            MouseDown += Vertex_MouseDown!;
            MouseMove += Vertex_MouseMove!;
            MouseUp += Vertex_MouseUp!;
        }
        public Point Center()
        {
            return new Point(Location.X + Width / 2, Location.Y + Height / 2);
        }
        public void Vertex_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var brush = new SolidBrush(Color.Red);
            if (Type == VertexType.G1) brush.Color = Color.Green;
            if (Type == VertexType.C1) brush.Color = Color.Purple;
            using var pen = new Pen(Color.Black);

            g.FillEllipse(brush, 0, 0, Width - 1, Height - 1);
            g.DrawEllipse(pen, 0, 0, Width - 1, Height - 1);
        }
        public void Vertex_MouseDown(object? sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            Point mouse = Parent.PointToClient(Cursor.Position);
            if (e.Button == MouseButtons.Left && ModifierKeys.HasFlag(Keys.Control) && Parent is Polygon polygon1) polygon1.MCommands.MouseDownPolygon(mouse, true, null);
            else if (ModifierKeys.HasFlag(Keys.Control) && e.Button == MouseButtons.Left && Parent is Polygon polygon2) polygon2.MCommands.MouseDownPolygon(mouse, true, null);
            else if (e.Button == MouseButtons.Left && Parent is Polygon polygon3) polygon3.MCommands.MouseDownPolygon(mouse, false, this);
            else if (e.Button == MouseButtons.Right) VRightClickMenu.Menu.Show(this, e.Location);
            Capture = true;
        }
        public void Vertex_MouseMove(object? sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            if (Parent is Polygon polygon && polygon.IsDragging)
            {
                Point mouse = Parent.PointToClient(Cursor.Position);
                polygon.MCommands.MouseMovePolygon(mouse, this);
            }
        }
        public void Vertex_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Parent is Polygon polygon && polygon.IsDragging)
            {
                polygon.MCommands.MouseUpPolygon();
                Capture = false;
            }
        }
    }
}