using Grafika.Constraints;
using Grafika.Controls;
using Grafika.ExtraCommands;

namespace Grafika.ExtraConstraints
{
    internal class G1BezierVertical : IG1BezierConstraint
    {
        public VerticalConstraint Vertical = new();
        public void ApplyMove(BezierSegment dragged, Vertex C1, Vertex toDrag, double desLength)
        {
            C1.Location = new(dragged.Location.X, C1.Location.Y);
            Vertical.ApplyMove(C1, toDrag);
            RepairG1.Repair(dragged);
            dragged.Location = new(dragged.Location.X + dragged.Width / 4, dragged.Location.Y);
        }
    }
}