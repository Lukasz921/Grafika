namespace Proj1.Components
{
    internal class Vertex : PictureBox
    {
        private bool IsDragging = false;
        private Point MouseOffset = Point.Empty;
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
            if (e.Button == MouseButtons.Left && ModifierKeys.HasFlag(Keys.Control) && Parent is Polygon polygon1)
            {
                Point mouse = Parent.PointToClient(Cursor.Position);
                polygon1.MouseDownPolygon(mouse);
                Capture = true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                Point mouse = Parent.PointToClient(Cursor.Position);
                MouseOffset = new Point(mouse.X - Location.X, mouse.Y - Location.Y);
                IsDragging = true;
                Capture = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Parent is Polygon polygon2)
                {
                    polygon2.RemoveVertexOnMiddle(this);
                    polygon2.Invalidate();
                }
            }
        }
        private void Vertex_MouseMove(object? sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            if (Parent is Polygon poly && poly.IsDragging)
            {
                Point mouse = Parent.PointToClient(Cursor.Position);
                poly.MouseMovePolygon(mouse);
                return;
            }
            if (!IsDragging) return;

            Point mousePos = Parent.PointToClient(Cursor.Position);
            Point newLocation = new(mousePos.X - MouseOffset.X, mousePos.Y - MouseOffset.Y);

            int maxX = Parent.ClientSize.Width - Width;
            int maxY = Parent.ClientSize.Height - Height;
            if (newLocation.X < 0) newLocation.X = 0;
            else if (newLocation.X > maxX) newLocation.X = maxX;
            if (newLocation.Y < 0) newLocation.Y = 0;
            else if (newLocation.Y > maxY) newLocation.Y = maxY;

            Location = newLocation;
            Parent?.Invalidate();
        }
        private void Vertex_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Parent is Polygon poly && poly.IsDragging)
            {
                poly.MouseUpPolygon();
                Capture = false;
                return;
            }
            if (e.Button != MouseButtons.Left) return;
            IsDragging = false;
            Capture = false;
        }
    }
}