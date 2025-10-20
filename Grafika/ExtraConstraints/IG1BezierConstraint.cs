using Grafika.Controls;

namespace Grafika.ExtraConstraints
{
    internal interface IG1BezierConstraint
    {
        void ApplyMove(BezierSegment dragged, Vertex C1, Vertex toDrag, double desLength);
    }
}