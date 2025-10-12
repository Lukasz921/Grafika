namespace WinFormsApp1.Elementy
{
    internal static class Help
    {
        public static (Edge A, Edge B) GetConnectedEdges(GraphPanel gp, Vertex v)
        {
            Edge? A = null;
            Edge? B = null;
            foreach (Edge edge in gp.Edges)
            {
                if ((edge.A == v || edge.B == v) && A == null) A = edge;
                else if (edge.A == v || edge.B == v) B = edge;
            }
            return (A, B)!;
        }
        public static List<Vertex> GetConstChain(GraphPanel gp, Vertex v)
        {
            var result = new List<Vertex>();
            if (gp == null) return result;
            var visited = new HashSet<Vertex> { v };
            result.Add(v);
            var cur = v;
            while (true)
            {
                var nextEdge = gp.Edges.FirstOrDefault(e => e.Modifier == EdgeModifier.Const && e.Connects(cur) && !visited.Contains(e.Other(cur)));
                if (nextEdge == null) break;
                var next = nextEdge.Other(cur);
                visited.Add(next);
                result.Insert(0, next);
                cur = next;
            }
            cur = v;
            while (true)
            {
                var nextEdge = gp.Edges.FirstOrDefault(e => e.Modifier == EdgeModifier.Const && e.Connects(cur) && !visited.Contains(e.Other(cur)));
                if (nextEdge == null) break;
                var next = nextEdge.Other(cur);
                visited.Add(next);
                result.Add(next);
                cur = next;
            }
            return result;
        }
        public static bool TryGetAllowedMoveDirection(GraphPanel gp, List<Vertex> chain, out string mode)
        {
            mode = "Free";
            if (chain.Count < 2) return false;
            var left = chain.First();
            var right = chain.Last();
            (Edge leftA, Edge leftB) = GetConnectedEdges(gp, left);
            (Edge rightA, Edge rightB) = GetConnectedEdges(gp, right);
            var leftOuter = new[] { leftA, leftB }.FirstOrDefault(e => e.Modifier != EdgeModifier.Const);
            var rightOuter = new[] { rightA, rightB }.FirstOrDefault(e => e.Modifier != EdgeModifier.Const);
            var leftType = leftOuter?.Modifier ?? EdgeModifier.Normal;
            var rightType = rightOuter?.Modifier ?? EdgeModifier.Normal;
            if ((leftType == EdgeModifier.Vertical && rightType == EdgeModifier.Lock45) ||
                (leftType == EdgeModifier.Lock45 && rightType == EdgeModifier.Vertical))
            {
                mode = "None";
                return true;
            }
            if (leftType == EdgeModifier.Vertical || rightType == EdgeModifier.Vertical)
            {
                mode = "Vertical";
                return true;
            }
            if (leftType == EdgeModifier.Lock45 || rightType == EdgeModifier.Lock45)
            {
                mode = "Lock45";
                return true;
            }
            mode = "Free";
            return true;
        }
    }
}