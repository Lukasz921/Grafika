namespace WinFormsApp1.Elementy
{
    public enum EdgeType
    {
        Normal,
        SemiCircle,
        Bezier,
    }
    public enum EdgeModifier
    {
        Normal,
        Const,
        Vertical,
        Lock45,
    }
    public class Edge(Vertex a, Vertex b)
    {
        public Vertex A { get; } = a;
        public Vertex B { get; } = b;
        public EdgeType Type { get; set; } = EdgeType.Normal;
        public EdgeModifier Modifier { get; set; } = EdgeModifier.Normal;
        public bool Connects(Vertex v) => A == v || B == v;
        public double Length()
        {
            double dx = A.Center.X - B.Center.X;
            double dy = A.Center.Y - B.Center.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public Vertex Other(Vertex v) => A == v ? B : A;
    }
}