namespace Proj1.Components
{
    internal partial class Polygon
    {
        public (List<Vertex>, List<Vertex>) GetMoveChain(Vertex start)
        {
            List<Vertex> rightchain = [];
            rightchain.Add(start);
            while (true)
            {
                if (rightchain[^1] != start && (rightchain[^1].RightEdge!.V1 == start || rightchain[^1].RightEdge!.V2 == start)) break;
                if (rightchain[^1].RightEdge!.Modifier == EdgeModifier.Normal) break;
                if (rightchain[^1].RightEdge!.V2 == rightchain[^1]) rightchain.Add(rightchain[^1].RightEdge!.V1);
                else rightchain.Add(rightchain[^1].RightEdge!.V2);
            }
            List<Vertex> leftchain = [];

            if (rightchain.Count == Vertices.Count)
            {
                rightchain.RemoveAt(0);
                if (start.LeftEdge!.V2 == start) leftchain.Add(start.LeftEdge!.V1);
                else leftchain.Add(start.LeftEdge!.V2);
                return (leftchain, rightchain);
            }

            leftchain.Add(start);
            while (true)
            {
                if (leftchain[^1] != start && (leftchain[^1].LeftEdge!.V1 == start || leftchain[^1].LeftEdge!.V2 == start)) break;
                if (leftchain[^1].LeftEdge!.Modifier == EdgeModifier.Normal) break;
                if (leftchain[^1].LeftEdge!.V2 == leftchain[^1]) leftchain.Add(leftchain[^1].LeftEdge!.V1);
                else leftchain.Add(leftchain[^1].LeftEdge!.V2);
            }
            rightchain.RemoveAt(0);
            leftchain.RemoveAt(0);
            return (leftchain, rightchain);
        }
        public Edge FindEdge(Vertex v1, Vertex v2)
        {
            if (v1.RightEdge != null && (v1.RightEdge.V1 == v2 || v1.RightEdge.V2 == v2)) return v1.RightEdge;
            else if (v1.LeftEdge != null) return v1.LeftEdge;
            return Edges[0];
        }
        public void ApplyMove(Vertex dragged, Vertex toDrag)
        {
            Edge e = FindEdge(dragged, toDrag);
            if (e.Modifier == EdgeModifier.Const) ApplyConst(dragged, toDrag, e);
            if (e.Modifier == EdgeModifier.Vertical) ApplyVertical(dragged, toDrag);
            if (e.Modifier == EdgeModifier.Lock45) ApplyLock45(dragged, toDrag);
            Invalidate();
        }
        public static void ApplyConst(Vertex dragged, Vertex toDrag, Edge e)
        {
            double desiredLen = e.ConstLength;
            if (desiredLen <= 0) return;

            Point curCenter = toDrag.Center();

            double vx = curCenter.X - dragged.Center().X;
            double vy = curCenter.Y - dragged.Center().Y;

            double dist = Math.Sqrt(vx * vx + vy * vy);
            double ux = vx / dist;
            double uy = vy / dist;

            double newCenterX = dragged.Center().X + ux * desiredLen;
            double newCenterY = dragged.Center().Y + uy * desiredLen;
            int newLocationX = (int)Math.Round(newCenterX - toDrag.Width / 2.0);
            int newLocationY = (int)Math.Round(newCenterY - toDrag.Height / 2.0);

            toDrag.Location = new(newLocationX, newLocationY);
        }
        public static void ApplyVertical(Vertex dragged, Vertex toDrag)
        {
            toDrag.Location = new(dragged.Location.X, toDrag.Location.Y);
        }
        public static void ApplyLock45(Vertex dragged, Vertex toDrag)
        {
            int b = dragged.Center().Y + dragged.Center().X;
            Point curCenter = toDrag.Center();
            double avg = (curCenter.X - curCenter.Y + b) / 2.0;
            double newX = avg;
            double newY = -newX + b;
            int locX = (int)Math.Round(newX - toDrag.Width / 2.0);
            int locY = (int)Math.Round(newY - toDrag.Height / 2.0);
            toDrag.Location = new(locX, locY);
        }
    }
}