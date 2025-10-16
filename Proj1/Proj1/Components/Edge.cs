namespace Proj1.Components
{
    enum EdgeType
    {
        Normal,
        SemiCircle,
        Bezier,
    }
    enum EdgeModifier
    {
        Normal,
        Const,
        Lock45,
        Vertical,
    }
    internal class Edge(Vertex v1, Vertex v2)
    {
        public Vertex V1 { get; set; } = v1;
        public Vertex V2 { get; set; } = v2;
        public int ConstLength { get; set; } = 0;
        public int CurrLength()
        {
            return (int)Math.Sqrt((V1.Center().X - V2.Center().X) * (V1.Center().X - V2.Center().X) + (V1.Center().Y - V2.Center().Y) * (V1.Center().Y - V2.Center().Y));
        }
        public EdgeType Type { get; set; } = EdgeType.Normal;
        public EdgeModifier Modifier { get; set; } = EdgeModifier.Normal;
    }
}