namespace Proj1.Components
{
    internal static class Visuals
    {
        public static void BresenhamLine(Graphics g, Edge e, Brush brush)
        {
            int x0 = e.V1.Center().X;
            int y0 = e.V1.Center().Y;
            int x1 = e.V2.Center().X;
            int y1 = e.V2.Center().Y;

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
        public static void SemiCircleG0(Graphics g, Edge e, Pen p1, Pen p2)
        {
            var a = e.V1.Center();
            var b = e.V2.Center();
            float cx = (a.X + b.X) / 2f;
            float cy = (a.Y + b.Y) / 2f;
            float r = (float)(Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) / 2.0);
            var rect = new RectangleF(cx - r, cy - r, r * 2f, r * 2f);
            double angleRad = Math.Atan2(a.Y - cy, a.X - cx);
            g.DrawArc(p1, rect, (float)(angleRad * 180.0 / Math.PI), 180f);
            g.DrawLine(p2, e.V1.Center(), e.V2.Center());
        }
        public static void SemiCircleG1(Graphics g, Edge e, Pen p1, Pen p2)
        {
            bool aIsG1 = e.V1!.Type == VertexType.G1;
            var P = aIsG1 ? e.V1.Center() : e.V2.Center();
            var Q = aIsG1 ? e.V2.Center() : e.V1.Center();
            var vP = aIsG1 ? (Vertex)e.V1 : (Vertex)e.V2;
            var tangent = TryGetAdjacentTangentSimple(vP, e);

            double nx = -tangent.Y, ny = tangent.X;

            double mx = (P.X + Q.X) / 2.0, my = (P.Y + Q.Y) / 2.0;
            double dx = Q.X - P.X, dy = Q.Y - P.Y;
            double perpBx = -dy, perpBy = dx;

            double A11 = nx, A12 = -perpBx, A21 = ny, A22 = -perpBy;
            double B1 = mx - P.X, B2 = my - P.Y;
            double det = A11 * A22 - A12 * A21;

            double s = (B1 * A22 - B2 * A12) / det;
            double cx = P.X + s * nx;
            double cy = P.Y + s * ny;
            double radius = Math.Sqrt((cx - P.X) * (cx - P.X) + (cy - P.Y) * (cy - P.Y));

            double angleP = Math.Atan2(P.Y - cy, P.X - cx);
            double angleQ = Math.Atan2(Q.Y - cy, Q.X - cx);
            double sweep = (angleQ - angleP) * 180.0 / Math.PI;
            while (sweep <= -180) sweep += 360;
            while (sweep > 180) sweep -= 360;

            var arcRect = new RectangleF((float)(cx - radius), (float)(cy - radius), (float)(2 * radius), (float)(2 * radius));
            g.DrawArc(p1, arcRect, (float)(angleP * 180.0 / Math.PI), (float)sweep);
            g.DrawLine(p2, e.V1.Center(), e.V2.Center());
        }
        private static PointF TryGetAdjacentTangentSimple(Vertex v, Edge currentEdge)
        {
            Edge other = v.LeftEdge != currentEdge ? v.LeftEdge! : v.RightEdge!;
            PointF dir = new(other.V1 == v ? other.V2.Center().X - v.Center().X : other.V1.Center().X - v.Center().X, other.V1 == v ? other.V2.Center().Y - v.Center().Y : other.V1.Center().Y - v.Center().Y);
            float len = (float)Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
            return new PointF(dir.X / len, dir.Y / len);
        }
        public static void DrawEdgeLabel(Graphics g, Edge e, string text)
        {
            if (text == null || text.Length == 0 || text == "") return;

            var font = SystemFonts.DefaultFont;
            var a = e.V1.Center();
            var b = e.V2.Center();

            float mx = (a.X + b.X) / 2f;
            float my = (a.Y + b.Y) / 2f;

            SizeF sz = g.MeasureString(text, font);

            RectangleF bg = new(mx - sz.Width / 2f - 4f, my - sz.Height / 2f - 2f, sz.Width + 8f,sz.Height + 4f);

            using var brushBg = new SolidBrush(Color.FromArgb(220, Color.White));
            using var penBorder = new Pen(Color.Black, 1f);
            using var brushText = new SolidBrush(Color.Black);

            g.FillRectangle(brushBg, bg);
            g.DrawRectangle(penBorder, Rectangle.Round(bg));

            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.DrawString(text, font, brushText, bg, sf);
        }
        public static string SwitchEdgeLabel(EdgeModifier mod, double constLength = 0)
        {
            return mod switch
            {
                EdgeModifier.Const => Math.Round(constLength, 1).ToString(),
                EdgeModifier.Vertical => "Vertical",
                EdgeModifier.Lock45 => "Lock45",
                _ => ""
            };
        }
    }
}