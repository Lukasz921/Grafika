using Grafika.Controls;
using Grafika.Elements;

namespace Grafika.Visuals
{
    internal class SemiCircle : IVisual
    {
        public void DrawEdge(Graphics g, Edge e, SolidBrush brush, Pen pen1, Pen pen2, Polygon polygon)
        {
            if (e.V1.Type == VertexType.G0 && e.V2.Type == VertexType.G0) SemiCircleG0(g, e, pen1, pen2);
            else SemiCircleG1(g, e, pen1, pen2);
        }
        public static void SemiCircleG0(Graphics g, Edge e, Pen pen1, Pen pen2)
        {
            var a = e.V1.Center();
            var b = e.V2.Center();
            float cx = (a.X + b.X) / 2f;
            float cy = (a.Y + b.Y) / 2f;
            float r = (float)(Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) / 2.0);
            var rect = new RectangleF(cx - r, cy - r, r * 2f, r * 2f);
            double angleRad = Math.Atan2(a.Y - cy, a.X - cx);
            g.DrawArc(pen1, rect, (float)(angleRad * 180.0 / Math.PI), 180f);
            g.DrawLine(pen2, e.V1.Center(), e.V2.Center());
        }
        public static void SemiCircleG1(Graphics g, Edge e, Pen pen1, Pen pen2)
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
            g.DrawArc(pen1, arcRect, (float)(angleP * 180.0 / Math.PI), (float)sweep);
            g.DrawLine(pen2, e.V1.Center(), e.V2.Center());
        }
        public static PointF TryGetAdjacentTangentSimple(Vertex v, Edge currentEdge)
        {
            Edge other = v.LeftEdge != currentEdge ? v.LeftEdge! : v.RightEdge!;
            PointF dir = new(other.V1 == v ? other.V2.Center().X - v.Center().X : other.V1.Center().X - v.Center().X, other.V1 == v ? other.V2.Center().Y - v.Center().Y : other.V1.Center().Y - v.Center().Y);
            float len = (float)Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
            return new PointF(dir.X / len, dir.Y / len);
        }
    }
}
