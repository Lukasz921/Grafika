namespace Proj1.Components
{
    internal partial class Polygon
    {
        public (List<Vertex>, List<Vertex>) GetMoveChain(Vertex start)
        {
            int count = 0;
            List<Vertex> rightchain = [];
            rightchain.Add(start);
            while (true)
            {
                if (count >= Vertices.Count) break;
                if (rightchain[^1].RightEdge!.Modifier == EdgeModifier.Normal) break;
                if (rightchain[^1].RightEdge!.V2 == rightchain[^1]) rightchain.Add(rightchain[^1].RightEdge!.V1);
                else rightchain.Add(rightchain[^1].RightEdge!.V2);
                count++;
            }
            List<Vertex> leftchain = [];
            leftchain.Add(start);
            while (true)
            {
                if (count >= Vertices.Count) break;
                if (leftchain[^1].LeftEdge!.Modifier == EdgeModifier.Normal) break;
                if (leftchain[^1].LeftEdge!.V2 == leftchain[^1]) leftchain.Add(leftchain[^1].LeftEdge!.V1);
                else leftchain.Add(leftchain[^1].LeftEdge!.V2);
                count++;
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
        }
        public void ApplyConst(Vertex dragged, Vertex toDrag, Edge e)
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

            toDrag.Location = new Point(newLocationX, newLocationY);
            Invalidate();
        }
        public void ApplyVertical(Vertex dragged, Vertex toDrag, Edge e)
        {

        }
        public void ApplyLock45(Vertex dragged, Vertex toDrag, Edge e)
        {

        }
    }
}