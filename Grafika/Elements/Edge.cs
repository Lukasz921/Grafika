using Grafika.Constraints;
using Grafika.Controls;
using Grafika.Visuals;

namespace Grafika.Elements
{
    internal class Edge(Vertex v1,  Vertex v2)
    {
        public Vertex V1 { get; set; } = v1;
        public Vertex V2 { get; set; } = v2;
        public Vertex OtherVertex(Vertex v)
        {
            if (v == V2) return V1;
            return V2;
        }
        public int ConstLength { get; set; } = 0;
        public IVisual Visual { get; set; } = new Bresenham();
        public IConstraint Constraint { get; set; } = new NoConstraint();
        public ILabel Label { get; set; } = new NormalLabel();
        public int CurrLength()
        {
            return (int)Math.Sqrt((V1.Center().X - V2.Center().X) * (V1.Center().X - V2.Center().X) + (V1.Center().Y - V2.Center().Y) * (V1.Center().Y - V2.Center().Y));
        }
    }
}