using Grafika.Controls;

namespace Grafika.Constraints
{
    internal class Lock45Constraint : IConstraint
    {
        public void ApplyMove(Vertex dragged, Vertex toDrag)
        {
            int b = dragged.Center().Y + dragged.Center().X;
            Point curCenter = toDrag.Center();
            double avg = (curCenter.X - curCenter.Y + b) / 2.0;
            double newX = avg;
            double newY = -newX + b;
            int locX = (int)Math.Round(newX - toDrag.Width / 2.0);
            int locY = (int)Math.Round(newY - toDrag.Height / 2.0);
            toDrag.Location = new(locX, locY);
        }
    }
}