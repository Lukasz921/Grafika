namespace WinFormsApp1.Elementy
{
    public class Vertex : PictureBox
    {
        private bool IsDraggingOne = false;
        private bool IsChainDragging = false;
        private bool IsDraggingAll = false;
        private List<Vertex>? Chain = null;
        private Point LastMouseForChain;
        private Point LastMouseForAll;
        private Point MouseOffset;
        public Point Center => new(Location.X + Width / 2, Location.Y + Height / 2);
        public int SizE { get; set; } = 26;
        public Vertex()
        {
            Size = new Size(SizE, SizE);
            BackColor = Color.Transparent;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Paint += Vertex_Paint!;
            MouseDown += Vertex_MouseDown!;
            MouseMove += Vertex_MouseMove!;
            MouseUp += Vertex_MouseUp!;
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
            if (Parent is not GraphPanel gp) return;

            if (e.Button == MouseButtons.Right)
            {
                gp.RemoveVertex(this);
                return;
            }
            (Edge A, Edge B) = Help.GetConnectedEdges(gp, this);
            if ((A.Modifier == EdgeModifier.Vertical && B.Modifier == EdgeModifier.Lock45) || (A.Modifier == EdgeModifier.Lock45 && B.Modifier == EdgeModifier.Vertical)) return;
            if (e.Button == MouseButtons.Left)
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    IsDraggingAll = true;
                    LastMouseForAll = gp.PointToClient(Cursor.Position);
                    Capture = true;
                    return;
                }
                var c = Help.GetConstChain(gp, this);
                if (c.Count > 1)
                {
                    IsChainDragging = true;
                    LastMouseForChain = gp.PointToClient(Cursor.Position);
                    Chain = c;
                    Capture = true;
                    return;
                }
                IsDraggingOne = true;
                MouseOffset = new Point(e.X, e.Y);
                Capture = true;
            }
        }
        private void Vertex_MouseMove(object? sender, MouseEventArgs e)
        {
            if (Parent is not GraphPanel gp) return;
            if (IsDraggingOne)
            {
                Point cursorPos = Parent.PointToClient(Cursor.Position);
                Point desiredTopLeft = new(cursorPos.X - MouseOffset.X, cursorPos.Y - MouseOffset.Y);
                var verticalEdge = gp.Edges.FirstOrDefault(ed => ed.Connects(this) && ed.Modifier == EdgeModifier.Vertical);
                if (verticalEdge != null)
                {
                    var other = verticalEdge.Other(this);
                    int targetCenterX = other.Center.X;
                    desiredTopLeft.X = targetCenterX - (Width / 2);
                }
                var lock45Edge = gp.Edges.FirstOrDefault(ed => ed.Connects(this) && ed.Modifier == EdgeModifier.Lock45);
                if (lock45Edge != null)
                {
                    var other = lock45Edge.Other(this);
                    Point otherCenter = other.Center;
                    Point desiredCenter = new(cursorPos.X - MouseOffset.X + Width / 2, cursorPos.Y - MouseOffset.Y + Height / 2);
                    double ux = 1.0 / Math.Sqrt(2.0);
                    double uy = -1.0 / Math.Sqrt(2.0);
                    double wx = desiredCenter.X - otherCenter.X;
                    double wy = desiredCenter.Y - otherCenter.Y;
                    double t = wx * ux + wy * uy;
                    int newCenterX = otherCenter.X + (int)Math.Round(ux * t);
                    int newCenterY = otherCenter.Y + (int)Math.Round(uy * t);
                    desiredTopLeft = new Point(newCenterX - Width / 2, newCenterY - Height / 2);
                }
                Location = desiredTopLeft;
                gp.Invalidate();
                return;
            }
            if (IsChainDragging && Chain != null)
            {
                Point curr = gp.PointToClient(Cursor.Position);
                int dx = curr.X - LastMouseForChain.X;
                int dy = curr.Y - LastMouseForChain.Y;
                if (dx == 0 && dy == 0) return;
                Help.TryGetAllowedMoveDirection(gp, Chain, out string mode);
                switch (mode)
                {
                    case "None":
                        return;
                    case "Vertical":
                        dx = 0;
                        break;
                    case "Lock45":
                        dy = -dx;
                        break;
                }
                foreach (var v in Chain) v.Location = new Point(v.Location.X + dx, v.Location.Y + dy);
                LastMouseForChain = curr;
                gp.Invalidate();
                return;
            }
            if (IsDraggingAll)
            {
                Point curr = gp.PointToClient(Cursor.Position);
                int dx = curr.X - LastMouseForAll.X;
                int dy = curr.Y - LastMouseForAll.Y;
                if (dx == 0 && dy == 0) return;
                foreach (var v in gp.Vertices) v.Location = new Point(v.Location.X + dx, v.Location.Y + dy);
                LastMouseForAll = curr;
                gp.Invalidate();
            }
        }
        private void Vertex_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (IsDraggingOne)
            {
                IsDraggingOne = false;
                Capture = false;
            }
            if (IsChainDragging)
            {
                IsChainDragging = false;
                Chain = null;
                Capture = false;
            }
            if (IsDraggingAll)
            {
                IsDraggingAll = false;
                Capture = false;
            }
        }
    }
}