using Grafika.Controls;

namespace Grafika.Constraints
{
    internal class VerticalConstraint : IConstraint
    {
        public void ApplyMove(Vertex dragged, Vertex toDrag)
        {
            toDrag.Location = new(dragged.Location.X, toDrag.Location.Y);
        }
    }
}