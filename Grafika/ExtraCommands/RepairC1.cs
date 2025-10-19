using Grafika.Controls;
using Grafika.Elements;

namespace Grafika.ExtraCommands
{
    internal static class RepairC1
    {
        public static void Repair(List<(BezierSegment, BezierSegment, Edge)> segments)
        {
            foreach(var segment in segments)
            {
                Vertex? c1 = null;
                if (segment.Item3.V1.Type == VertexType.C1) c1 = segment.Item3.V1;
                if (segment.Item3.V2.Type == VertexType.C1) c1 = segment.Item3.V2;
                if (c1 == null) continue;

                Vertex? v;
                BezierSegment? b1;

                if (segment.Item1.LeftVertex == c1)
                {
                    b1 = segment.Item1;
                    v = c1.LeftEdge!.OtherVertex(c1);
                }
                else
                {
                    b1 = segment.Item2;
                    v = c1.RightEdge!.OtherVertex(c1);
                }
                if (b1 == null) continue;
                if (v == null) continue;

                var cC = c1.Center();
                var vC = v.Center();
                double dx = cC.X - vC.X;
                double dy = cC.Y - vC.Y;
                double dist = Math.Sqrt(dx * dx + dy * dy);
                if (dist < 1e-6) continue;
                double ux = dx / dist;
                double uy = dy / dist;
                double target = 3.0 * dist;
                double bx = cC.X + ux * target;
                double by = cC.Y + uy * target;
                int locX = (int)Math.Round(bx - b1.Width / 2.0);
                int locY = (int)Math.Round(by - b1.Height / 2.0);
                b1.Location = new Point(locX, locY);
            }
        }
        public static void Repair(BezierSegment b1)
        {
            Vertex? c1 = null;
            Vertex? v = null;
            if (b1.LeftVertex != null && b1.LeftVertex.Type == VertexType.C1)
            {
                c1 = b1.LeftVertex;
                v = c1.LeftEdge!.OtherVertex(c1);
            }
            if (b1.RightVertex != null && b1.RightVertex.Type == VertexType.C1)
            {
                c1 = b1.RightVertex;
                v = c1.RightEdge!.OtherVertex(c1);
            }
            if (c1 == null) return;
            if (v == null) return;

            var bC = b1.Center();
            var cC = c1.Center();
            double dx = cC.X - bC.X;
            double dy = cC.Y - bC.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            if (dist < 1e-6) return;
            double ux = dx / dist;
            double uy = dy / dist;
            double target = dist / 3.0;
            double vCenterX = cC.X + ux * target;
            double vCenterY = cC.Y + uy * target;
            int locX = (int)Math.Round(vCenterX - v.Width / 2.0);
            int locY = (int)Math.Round(vCenterY - v.Height / 2.0);
            v.Location = new Point(locX, locY);
        }
    }
}