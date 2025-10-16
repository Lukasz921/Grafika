namespace Proj1.Components
{
    internal class Vertex : PictureBox
    {
        public Edge? LeftEdge { get; set; } = null;
        public Edge? RightEdge { get; set; } = null;
        public Vertex(Point p)
        {
            Width = 24;
            Height = 24;
            Size = new Size(Width, Height);
            Location = new(p.X - Width / 2, p.Y - Height / 2);

            BackColor = Color.Transparent;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            Paint += Vertex_Paint!;
            MouseDown += Vertex_MouseDown!;
            MouseMove += Vertex_MouseMove!;
            MouseUp += Vertex_MouseUp!;
        }
        public Point Center()
        {
            return new(Location.X + Width / 2, Location.Y + Height / 2);
        }
        private void Vertex_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.FillEllipse(Brushes.Red, 0, 0, Width - 1, Height - 1);
            g.DrawEllipse(Pens.Black, 0, 0, Width - 1, Height - 1);
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
            else if (e.Button == MouseButtons.Right && Parent is Polygon polygon3)
            {
                polygon3.RemoveVertexOnMiddle(this);
                polygon3.Invalidate();
            }
        }

        private void Vertex_MouseMove(object? sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            if (Parent is Polygon poly && poly.IsDragging)
            {
                Point mouse = Parent.PointToClient(Cursor.Position);
                poly.MouseMovePolygon(mouse, this);
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