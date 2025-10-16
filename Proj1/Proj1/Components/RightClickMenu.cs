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

            var type = new ToolStripMenuItem("Types");
            var typenormal = new ToolStripMenuItem("Normal");
            var typesemicircle = new ToolStripMenuItem("Semicircle");
            var typebezier = new ToolStripMenuItem("Bezier");
            typenormal.Click += (_, __) => { if (Hit != null) ApplyTypeNormal(); Hit = null; };
            typesemicircle.Click += (_, __) => { if (Hit != null) ApplyTypeSemiCircle(); Hit = null; };
            typebezier.Click += (_, __) => { if (Hit != null) ApplyTypeBezier(); Hit = null; };
            type.DropDownItems.Add(typenormal);
            type.DropDownItems.Add(typesemicircle);
            type.DropDownItems.Add(typebezier);
            Menu.Items.Add(type);

            var modifiers = new ToolStripMenuItem("Modifiers");
            var normalmodifier = new ToolStripMenuItem("Normal");
            var constmodifier = new ToolStripMenuItem("Const");
            var verticalmodifier = new ToolStripMenuItem("Vertical");
            var lock45modifier = new ToolStripMenuItem("Lock45");
            normalmodifier.Click += (_, __) => { if (Hit != null) ApplyModifierNormal(); Hit = null; };
            constmodifier.Click += (_, __) => { if (Hit != null) ApplyModifierConst(); Hit = null; };
            verticalmodifier.Click += (_, __) => { if (Hit != null) ApplyModifierVertical(); Hit = null; };
            lock45modifier.Click += (_, __) => { if (Hit != null) ApplyModifierLock45(); Hit = null; };
            modifiers.DropDownItems.Add(normalmodifier);
            modifiers.DropDownItems.Add(constmodifier);
            modifiers.DropDownItems.Add(verticalmodifier);
            modifiers.DropDownItems.Add(lock45modifier);
            Menu.Items.Add(modifiers);

            var add = new ToolStripMenuItem("Add vertex");
            add.Click += (_, __) => { if (Hit != null) AddVertex(); };
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
        private void AddVertex()
        {
            Owner.AddVertexOnMiddle(Hit!);
            Owner.Invalidate();
            Hit = null;
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
        private void ApplyModifierNormal()
        {
            Hit!.Modifier = EdgeModifier.Normal;
            Hit!.ConstLength = 0;
            Owner.Invalidate();
        }
        private void ApplyModifierConst()
        {
            Hit!.Modifier = EdgeModifier.Const;
            Hit!.ConstLength = Hit!.CurrLength();
            Owner.Invalidate();
        }
        private void ApplyModifierVertical()
        {
            Hit!.Modifier = EdgeModifier.Vertical;
            Hit!.ConstLength = 0;
            Owner.Invalidate();
        }
        private void ApplyModifierLock45()
        {
            Hit!.Modifier = EdgeModifier.Lock45;
            Hit!.ConstLength = 0;
            Owner.Invalidate();
        }
    }
}