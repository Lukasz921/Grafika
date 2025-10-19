using Grafika.Controls;

namespace Grafika.Constraints
{
    internal interface IConstraint
    {
        void ApplyMove(Vertex dragged, Vertex toDragged);
    }
}