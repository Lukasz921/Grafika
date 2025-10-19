using Grafika.Controls;

namespace Grafika.Constraints
{
    internal class NoConstraint : IConstraint
    {
        public void ApplyMove(Vertex dragged, Vertex toDrag) { }
    }
}