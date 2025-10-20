using Grafika.Controls;
using Grafika.ExtraCommands;

namespace Grafika.ExtraConstraints
{
    internal class C1BezierNoConstraint : IC1BezierConstraint
    {
        public void ApplyMove(BezierSegment dragged, Vertex C1, Vertex toDrag, double desLength)
        {
            RepairC1.Repair(dragged);
        }
    }
}