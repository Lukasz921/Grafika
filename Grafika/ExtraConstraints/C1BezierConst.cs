using Grafika.Constraints;
using Grafika.Controls;
using Grafika.ExtraCommands;

namespace Grafika.ExtraConstraints
{
    internal class C1BezierConst : IC1BezierConstraint
    {
        public ConstConstraint Const = new();
        public void ApplyMove(BezierSegment dragged, Vertex C1, Vertex toDrag, double desLength)
        {
            Point curCenter = C1.Center();

            double vx = curCenter.X - dragged.Center().X;
            double vy = curCenter.Y - dragged.Center().Y;

            double dist = Math.Sqrt(vx * vx + vy * vy);
            double ux = vx / dist;
            double uy = vy / dist;

            double newCenterX = dragged.Center().X + ux * desLength;
            double newCenterY = dragged.Center().Y + uy * desLength;
            int newLocationX = (int)Math.Round(newCenterX - C1.Width / 2.0);
            int newLocationY = (int)Math.Round(newCenterY - C1.Height / 2.0);

            C1.Location = new(newLocationX, newLocationY);
            Const.ApplyMove(C1, toDrag);
            RepairC1.Repair(dragged);
        }
    }
}