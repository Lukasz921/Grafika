using Grafika.Constraints;
using Grafika.Controls;
using Grafika.ExtraCommands;

namespace Grafika.ExtraConstraints
{
    internal class C1BezierLock45 : IC1BezierConstraint
    {
        public Lock45Constraint Lock45 = new();
        public void ApplyMove(BezierSegment dragged, Vertex C1, Vertex toDrag, double desLength)
        {
            int b = dragged.Center().Y + dragged.Center().X;
            Point curCenter = C1.Center();
            double avg = (curCenter.X - curCenter.Y + b) / 2.0;
            double newX = avg;
            double newY = -newX + b;
            int locX = (int)Math.Round(newX - C1.Width / 2.0);
            int locY = (int)Math.Round(newY - C1.Height / 2.0);
            C1.Location = new(locX, locY);
            Lock45.ApplyMove(C1, toDrag);
            RepairC1.Repair(dragged);
        }
    }
}