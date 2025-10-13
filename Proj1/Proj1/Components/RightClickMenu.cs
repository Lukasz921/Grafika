namespace Proj1.Components
{
    internal class RightClickMenu
    {
        private readonly Polygon Owner;
        private readonly ContextMenuStrip Menu;
        private Edge? Hit;

        public RightClickMenu(Polygon owner)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Menu = new ContextMenuStrip();

            var typesItem = new ToolStripMenuItem("Types");
            var normalItem = new ToolStripMenuItem("Normal");
            var semiItem = new ToolStripMenuItem("Semicircle");
            var bezierItem = new ToolStripMenuItem("Bezier");

            normalItem.Click += (_, __) => { if (Hit != null) ApplyTypeNormal(); Hit = null; };
            semiItem.Click += (_, __) => { if (Hit != null) ApplyTypeSemiCircle(); Hit = null; };
            bezierItem.Click += (_, __) => { if (Hit != null) ApplyTypeBezier(); Hit = null; };

            typesItem.DropDownItems.Add(normalItem);
            typesItem.DropDownItems.Add(semiItem);
            typesItem.DropDownItems.Add(bezierItem);
            Menu.Items.Add(typesItem);
            var modifiersItem = new ToolStripMenuItem("Modifiers");
            Menu.Items.Add(modifiersItem);
            var add = new ToolStripMenuItem("Add vertex");
            add.Click += (_, __) =>
            {
                if (Hit != null)
                {
                    Owner.AddVertexOnMiddle(Hit);
                    Owner.Invalidate();
                    Hit = null;
                }
            };
            Menu.Items.Add(add);

            Owner.MouseDown += Owner_MouseDown;
        }
        private void Owner_MouseDown(object? s, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var p = e.Location;
            Hit = HitTestEdge(p, 6);
            if (Hit != null) Menu.Show(Owner, p);
        }
        private Edge? HitTestEdge(Point p, int thresh)
        {
            foreach (var edge in Owner.Edges)
            {
                var a = edge.V1.Center();
                var b = edge.V2.Center();
                if (DistancePointToSegment(p, a, b) <= thresh) return edge;
            }
            return null;
        }
        private static double DistancePointToSegment(Point p, Point a, Point b)
        {
            double px = p.X, py = p.Y;
            double ax = a.X, ay = a.Y;
            double bx = b.X, by = b.Y;
            double dx = bx - ax, dy = by - ay;
            if (dx == 0 && dy == 0) return Math.Sqrt((px - ax) * (px - ax) + (py - ay) * (py - ay));
            double t = ((px - ax) * dx + (py - ay) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));
            double projX = ax + t * dx, projY = ay + t * dy;
            double dx2 = px - projX, dy2 = py - projY;
            return Math.Sqrt(dx2 * dx2 + dy2 * dy2);
        }
        private void ApplyTypeNormal()
        {
            Hit!.Type = EdgeType.Normal;
            Owner.Invalidate();
        }
        private void ApplyTypeSemiCircle()
        {
            Hit!.Type = EdgeType.SemiCircle;
            Owner.Invalidate();
        }
        private void ApplyTypeBezier()
        {
            Hit!.Type = EdgeType.Bezier;
            Owner.Invalidate();
        }
    }
}
