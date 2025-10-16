namespace Proj1.Components
{
    enum VertexType
    {
        G0,
        G1,
        C1,
    }
    internal class Vertex : Control
    {
        private readonly ContextMenuStrip Menu;
        public Edge? LeftEdge { get; set; } = null;
        public Edge? RightEdge { get; set; } = null;
        public VertexType Type { get; set; } = VertexType.G0;
        public Vertex(Point p)
        {
            Width = 24;
            Height = 24;
            Location = new Point(p.X - Width / 2, p.Y - Height / 2);
            SetStyle(ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            Menu = new ContextMenuStrip();

            ToolStripMenuItem remove = new("Remove vertex");
            remove.Click += (_, __) => 
            { 
                if (Parent is Polygon polygon)
                {
                    polygon.RemoveVertexOnMiddle(this);
                    polygon.Invalidate();
                }
            };
            Menu.Items.Add(remove);

            ToolStripMenuItem modifiers = new("Modifiers");
            ToolStripMenuItem normalmodifier = new("Normal");
            ToolStripMenuItem g1modifier = new("G1");
            ToolStripMenuItem c1modifier = new("C1");
            normalmodifier.Click += (_, __) =>
            {
                if (Parent is Polygon polygon)
                {
                    Type = VertexType.G0;
                    LeftEdge!.Modifier = EdgeModifier.Normal;
                    RightEdge!.Modifier = EdgeModifier.Normal;
                }
            };
            g1modifier.Click += (_, __) =>
            {
                if (Parent is Polygon polygon)
                {
                    Vertex v1 = LeftEdge!.V1;
                    if (v1 == this) v1 = LeftEdge!.V2;
                    Vertex v2 = RightEdge!.V1;
                    if (v2 == this) v2 = RightEdge!.V2;
                    if (v1.Type == VertexType.G1 || v2.Type == VertexType.G1) return;

                    bool b = true;
                    if (LeftEdge!.Type == EdgeType.SemiCircle && RightEdge!.Type == EdgeType.Normal) b = false;
                    if (LeftEdge!.Type == EdgeType.Normal && RightEdge!.Type == EdgeType.SemiCircle) b = false;
                    if (b) return;

                    Type = VertexType.G1;
                    LeftEdge!.Modifier = EdgeModifier.Normal;
                    RightEdge!.Modifier = EdgeModifier.Normal;

                    polygon.Invalidate();
                }
            };
            c1modifier.Click += (_, __) => { };
            modifiers.DropDownItems.Add(normalmodifier);
            modifiers.DropDownItems.Add(g1modifier);
            modifiers.DropDownItems.Add(c1modifier);
            Menu.Items.Add(modifiers);

            Paint += Vertex_Paint;
            MouseDown += Vertex_MouseDown!;
            MouseMove += Vertex_MouseMove!;
            MouseUp += Vertex_MouseUp!;
        }
        public Point Center()
        {
            return new Point(Location.X + Width / 2, Location.Y + Height / 2);
        }
        private void Vertex_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var brush = new SolidBrush(Color.Red);
            using var pen = new Pen(Color.Black);
            g.FillEllipse(brush, 0, 0, Width - 1, Height - 1);
            g.DrawEllipse(pen, 0, 0, Width - 1, Height - 1);
        }
        private void Vertex_MouseDown(object? sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            Point mouse = Parent.PointToClient(Cursor.Position);
            if (e.Button == MouseButtons.Left && ModifierKeys.HasFlag(Keys.Control) && Parent is Polygon polygon1)
            {
                polygon1.MouseDownPolygon(mouse, true, null);
                Capture = true;
            }
            else if (e.Button == MouseButtons.Left && Parent is Polygon polygon2)
            {
                polygon2.MouseDownPolygon(mouse, false, this);
                Capture = true;
            }
            else if (e.Button == MouseButtons.Right) Menu.Show(this, e.Location);
        }
        private void Vertex_MouseMove(object? sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            if (Parent is Polygon polygon && polygon.IsDragging)
            {
                Point mouse = Parent.PointToClient(Cursor.Position);
                polygon.MouseMovePolygon(mouse, this);
            }
        }
        private void Vertex_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Parent is Polygon poly && poly.IsDragging)
            {
                poly.MouseUpPolygon();
                Capture = false;
            }
        }
    }
}