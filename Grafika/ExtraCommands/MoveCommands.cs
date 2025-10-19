using Grafika.Controls;

namespace Grafika.ExtraCommands
{
    internal class MoveCommands(Polygon polygon)
    {
        public Polygon Polygon { get; set; } = polygon;
        public void MouseDownPolygon(Point mouse, bool isCtrlClicked, Vertex? dragged)
        {
            Polygon.IsDragging = true;
            Polygon.IsCtrlClicked = isCtrlClicked;
            if (Polygon.IsCtrlClicked) Polygon.MouseOffset = mouse;
            else
            {
                if (dragged != null) Polygon.MouseOffset = new Point(mouse.X - dragged.Location.X, mouse.Y - dragged.Location.Y);
                else Polygon.MouseOffset = mouse;
            }
        }
        public void MouseMovePolygon(Point mouse, Vertex dragged)
        {
            if (!Polygon.IsDragging) return;
            if (Polygon.IsCtrlClicked)
            {
                int dx = mouse.X - Polygon.MouseOffset.X;
                int dy = mouse.Y - Polygon.MouseOffset.Y;
                if (dx == 0 && dy == 0)
                {
                    Polygon.MouseOffset = mouse;
                    return;
                }
                foreach (Vertex v in Polygon.Vertices) v.Location = new Point(v.Location.X + dx, v.Location.Y + dy);
                foreach (var element in Polygon.Segments)
                {
                    element.Item1.Location = new Point(element.Item1.Location.X + dx, element.Item1.Location.Y + dy);
                    element.Item2.Location = new Point(element.Item2.Location.X + dx, element.Item2.Location.Y + dy);
                }
                Polygon.MouseOffset = new Point(Polygon.MouseOffset.X + dx, Polygon.MouseOffset.Y + dy);
            }
            else
            {
                Point newLocation = new(mouse.X - Polygon.MouseOffset.X, mouse.Y - Polygon.MouseOffset.Y);
                dragged.Location = newLocation;

                Vertex v = dragged;
                while (v.RightEdge!.OtherVertex(v) != dragged)
                {
                    v.RightEdge!.Constraint.ApplyMove(v, v.RightEdge!.OtherVertex(v));
                    v = v.RightEdge!.OtherVertex(v);
                }

                v = dragged;
                while (v.LeftEdge!.OtherVertex(v) != dragged)
                {
                    v.LeftEdge!.Constraint.ApplyMove(v, v.LeftEdge!.OtherVertex(v));
                    v = v.LeftEdge!.OtherVertex(v);
                }
            }
            Polygon.Invalidate();
        }
        public void MouseUpPolygon()
        {
            Polygon.IsDragging = false;
            Polygon.IsCtrlClicked = false;
        }
        public void MouseDownBezier(Point mouse, bool isCtrlClicked, BezierSegment? dragged)
        {
            Polygon.IsDragging = true;
            Polygon.IsCtrlClicked = isCtrlClicked;
            if (Polygon.IsCtrlClicked) Polygon.MouseOffset = mouse;
            else
            {
                if (dragged != null) Polygon.MouseOffset = new Point(mouse.X - dragged.Location.X, mouse.Y - dragged.Location.Y);
                else Polygon.MouseOffset = mouse;
            }
        }
        public void MouseMoveBezier(Point mouse, BezierSegment dragged)
        {
            if (!Polygon.IsDragging) return;
            if (Polygon.IsCtrlClicked)
            {
                int dx = mouse.X - Polygon.MouseOffset.X;
                int dy = mouse.Y - Polygon.MouseOffset.Y;
                if (dx == 0 && dy == 0)
                {
                    Polygon.MouseOffset = mouse;
                    return;
                }
                foreach (Vertex v in Polygon.Vertices) v.Location = new Point(v.Location.X + dx, v.Location.Y + dy);
                foreach (var element in Polygon.Segments)
                {
                    element.Item1.Location = new Point(element.Item1.Location.X + dx, element.Item1.Location.Y + dy);
                    element.Item2.Location = new Point(element.Item2.Location.X + dx, element.Item2.Location.Y + dy);
                }
                Polygon.MouseOffset = new Point(Polygon.MouseOffset.X + dx, Polygon.MouseOffset.Y + dy);
            }
            else
            {
                Point newLocation = new(mouse.X - Polygon.MouseOffset.X, mouse.Y - Polygon.MouseOffset.Y);
                dragged.Location = newLocation;
            }
            Polygon.Invalidate();
        }
        public void MouseUpBezier()
        {
            Polygon.IsDragging = false;
        }
    }
}