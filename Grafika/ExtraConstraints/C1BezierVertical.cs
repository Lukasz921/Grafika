using Grafika.Constraints;
using Grafika.Controls;
using Grafika.ExtraCommands;

namespace Grafika.ExtraConstraints
{
    internal class C1BezierVertical : IC1BezierConstraint
    {
        public VerticalConstraint Vertical = new();
        public void ApplyMove(BezierSegment dragged, Vertex C1, Vertex toDrag, double desLength)
        {
            C1.Location = new(dragged.Location.X, C1.Location.Y);
            Vertical.ApplyMove(C1, toDrag);
            RepairC1.Repair(dragged);
        }
    }
}