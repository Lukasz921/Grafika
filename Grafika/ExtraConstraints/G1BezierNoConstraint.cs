using Grafika.Controls;
using Grafika.ExtraCommands;

namespace Grafika.ExtraConstraints
{
    internal class G1BezierNoConstraint : IG1BezierConstraint
    {
        public void ApplyMove(BezierSegment dragged, Vertex C1, Vertex toDrag, double desLength)
        {
            RepairG1.Repair(dragged);
        }
    }
}