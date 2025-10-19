using Grafika.Controls;
using Grafika.Elements;

namespace Grafika.Constraints
{
    internal class ConstConstraint : IConstraint
    {
        public static Edge FindEdge(Vertex v1, Vertex v2)
        {
            if (v1.RightEdge != null && (v1.RightEdge.V1 == v2 || v1.RightEdge.V2 == v2)) return v1.RightEdge;
            else if (v1.LeftEdge != null) return v1.LeftEdge;
            return v1.RightEdge!;
        }
        public void ApplyMove(Vertex dragged, Vertex toDrag)
        {
            Edge e = FindEdge(toDrag, dragged);
            double desiredLen = e.ConstLength;
            if (desiredLen <= 0) return;

            Point curCenter = toDrag.Center();

            double vx = curCenter.X - dragged.Center().X;
            double vy = curCenter.Y - dragged.Center().Y;

            double dist = Math.Sqrt(vx * vx + vy * vy);
            double ux = vx / dist;
            double uy = vy / dist;

            double newCenterX = dragged.Center().X + ux * desiredLen;
            double newCenterY = dragged.Center().Y + uy * desiredLen;
            int newLocationX = (int)Math.Round(newCenterX - toDrag.Width / 2.0);
            int newLocationY = (int)Math.Round(newCenterY - toDrag.Height / 2.0);

            toDrag.Location = new(newLocationX, newLocationY);
        }
    }
}