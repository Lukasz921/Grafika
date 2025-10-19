namespace Grafika.Controls
{
    internal class BezierSegment : Control
    {
        public Vertex? LeftVertex { get; set; } = null;
        public Vertex? RightVertex { get; set; } = null;
        public BezierSegment(Point p)
        {
            Width = 14;
            Height = 14;
            Location = new Point(p.X - Width / 2, p.Y - Height / 2);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            Paint += BezierSegment_Paint;
            MouseDown += BezierSegment_MouseDown!;
            MouseMove += BezierSegment_MouseMove!;
            MouseUp += BezierSegment_MouseUp!;
        }
        public Point Center()
        {
            return new Point(Location.X + Width / 2, Location.Y + Height / 2);
        }
        public void BezierSegment_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var brush = new SolidBrush(Color.Blue);
            using var pen = new Pen(Color.Black);

            g.FillEllipse(brush, 0, 0, Width - 1, Height - 1);
            g.DrawEllipse(pen, 0, 0, Width - 1, Height - 1);
        }
        public void BezierSegment_MouseDown(object? sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            Point mouse = Parent.PointToClient(Cursor.Position);

            if (e.Button == MouseButtons.Left && ModifierKeys.HasFlag(Keys.Control) && Parent is Polygon polygon1)
            {
                polygon1.MCommands.MouseDownBezier(mouse, true, null);
                Capture = true;
            }
            else if (e.Button == MouseButtons.Left && Parent is Polygon polygon2)
            {
                polygon2.MCommands.MouseDownBezier(mouse, false, this);
                Capture = true;
            }
        }
        public void BezierSegment_MouseMove(object? sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            if (Parent is Polygon polygon && polygon.IsDragging)
            {
                Point mouse = Parent.PointToClient(Cursor.Position);
                polygon.MCommands.MouseMoveBezier(mouse, this);
            }
        }
        public void BezierSegment_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Parent is Polygon polygon && polygon.IsDragging)
            {
                polygon.MCommands.MouseUpBezier();
                Capture = false;
            }
        }
    }
}
