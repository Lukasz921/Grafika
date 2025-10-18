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
        public static void DrawBezier(Graphics g, Vertex v0, BezierSegment v1, BezierSegment v2, Vertex v3, Pen pen, int segments = 64)
        {
            var p0 = new PointF(v0.Center().X, v0.Center().Y);
            var p1 = new PointF(v1.Center().X, v1.Center().Y);
            var p2 = new PointF(v2.Center().X, v2.Center().Y);
            var p3 = new PointF(v3.Center().X, v3.Center().Y);
            if (segments < 1) segments = 1;
            float dt = 1f / segments;
            PointF prev = p0;
            for (int i = 1; i <= segments; i++)
            {
                float t = i * dt;
                float u = 1 - t;

                float b0 = u * u * u;
                float b1 = 3f * u * u * t;
                float b2 = 3f * u * t * t;
                float b3 = t * t * t;

                float x = b0 * p0.X + b1 * p1.X + b2 * p2.X + b3 * p3.X;
                float y = b0 * p0.Y + b1 * p1.Y + b2 * p2.Y + b3 * p3.Y;

                var cur = new PointF(x, y);
                g.DrawLine(pen, prev, cur);
                prev = cur;
            }
        }
        public static void DrawBezier(Graphics g, PointF p0, PointF p1, PointF p2, PointF p3, Pen pen, int segments = 64)
        {
            if (segments < 1) segments = 1;
            float dt = 1f / segments;
            PointF prev = p0;
            for (int i = 1; i <= segments; i++)
            {
                float t = i * dt;
                float u = 1 - t;
                float b0 = u * u * u;
                float b1 = 3f * u * u * t;
                float b2 = 3f * u * t * t;
                float b3 = t * t * t;
                float x = b0 * p0.X + b1 * p1.X + b2 * p2.X + b3 * p3.X;
                float y = b0 * p0.Y + b1 * p1.Y + b2 * p2.Y + b3 * p3.Y;
                PointF cur = new PointF(x, y);
                g.DrawLine(pen, prev, cur);
                prev = cur;
            }
        }
        private static PointF? TryGetAdjacentTangentSimpleAA(Vertex v, Edge edge)
        {
            Edge adj = (v.LeftEdge != null && v.LeftEdge != edge) ? v.LeftEdge
                     : (v.RightEdge != null && v.RightEdge != edge) ? v.RightEdge
                     : null!;
            if (adj == null) return null;
            Vertex other = (adj.V1 == v) ? adj.V2 : adj.V1;
            double dx = other.Center().X - v.Center().X;
            double dy = other.Center().Y - v.Center().Y;
            double len = Math.Sqrt(dx * dx + dy * dy);
            if (len < 1e-6) return null;
            return new PointF((float)(dx / len), (float)(dy / len));
        }
        public static void DrawBezierG1(Graphics g, Vertex v0, BezierSegment b1, BezierSegment b2, Vertex v3, Edge edge, Pen pen, int segments = 64)
        {
            // punkty aktualne
            var p0 = new PointF(v0.Center().X, v0.Center().Y);
            var p1 = new PointF(b1.Center().X, b1.Center().Y);
            var p2 = new PointF(b2.Center().X, b2.Center().Y);
            var p3 = new PointF(v3.Center().X, v3.Center().Y);

            // fallback: jeśli żaden z końców nie jest G1 -> normalny rysunek
            bool startG1 = v0.Type == VertexType.G1;
            bool endG1 = v3.Type == VertexType.G1;
            if (!startG1 && !endG1)
            {
                DrawBezier(g, p0, p1, p2, p3, pen, segments);
                return;
            }

            // nowe kontrolne punkty, kopiujemy istniejące i ewentualnie poprawiamy
            PointF newP1 = p1;
            PointF newP2 = p2;

            // helper do odległości
            double distP03 = Math.Sqrt((p3.X - p0.X) * (p3.X - p0.X) + (p3.Y - p0.Y) * (p3.Y - p0.Y));
            float defaultLen = (float)(distP03 / 3.0); // typowy rozkład (jeśli brak oryginalnej długości)

            // jeśli start jest G1 -> ustaw P1 wzdłuż tangenta z sąsiedniej krawędzi
            if (startG1)
            {
                var t = TryGetAdjacentTangentSimpleAA(v0, edge);
                if (t.HasValue)
                {
                    // len: preferuj istniejącą odległość p1-p0, jeśli sensowna
                    double curLen = Math.Sqrt((p1.X - p0.X) * (p1.X - p0.X) + (p1.Y - p0.Y) * (p1.Y - p0.Y));
                    float len = (curLen > 1e-3) ? (float)curLen : defaultLen;
                    newP1 = new PointF(p0.X + t.Value.X * len, p0.Y + t.Value.Y * len);
                }
                else
                {
                    // brak tangenta -> fallback do normalnego rysunku
                    DrawBezier(g, p0, p1, p2, p3, pen, segments);
                    return;
                }
            }

            // jeśli koniec jest G1 -> ustaw P2 tak, aby pochodna w t=1 była równoległa do tangenta sąsiedniej krawędzi
            if (endG1)
            {
                var t = TryGetAdjacentTangentSimpleAA(v3, edge);
                if (t.HasValue)
                {
                    // tangent returned points away from v3 along its adjacent edge.
                    // dla zakończenia P2 = P3 - tangent * len
                    double curLen = Math.Sqrt((p2.X - p3.X) * (p2.X - p3.X) + (p2.Y - p3.Y) * (p2.Y - p3.Y));
                    float len = (curLen > 1e-3) ? (float)curLen : defaultLen;
                    newP2 = new PointF(p3.X - t.Value.X * len, p3.Y - t.Value.Y * len);
                }
                else
                {
                    DrawBezier(g, p0, p1, p2, p3, pen, segments);
                    return;
                }
            }

            // rysuj krzywą z poprawionymi uchwytami
            DrawBezier(g, p0, newP1, newP2, p3, pen, segments);
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
            var vP = aIsG1 ? e.V1 : e.V2;
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