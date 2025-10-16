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

            ToolStripMenuItem add = new("Add vertex");
            add.Click += (_, __) => { if (Hit != null) AddVertex(); };
            Menu.Items.Add(add);

            ToolStripMenuItem type = new("Types");
            ToolStripMenuItem typenormal = new("Normal");
            ToolStripMenuItem typesemicircle = new("Semicircle");
            ToolStripMenuItem typebezier = new("Bezier");
            typenormal.Click += (_, __) => { if (Hit != null) ApplyTypeNormal(); Hit = null; };
            typesemicircle.Click += (_, __) => { if (Hit != null) ApplyTypeSemiCircle(); Hit = null; };
            typebezier.Click += (_, __) => { if (Hit != null) ApplyTypeBezier(); Hit = null; };
            type.DropDownItems.Add(typenormal);
            type.DropDownItems.Add(typesemicircle);
            type.DropDownItems.Add(typebezier);
            Menu.Items.Add(type);

            ToolStripMenuItem modifiers = new("Modifiers");
            ToolStripMenuItem normalmodifier = new("Normal");
            ToolStripMenuItem constmodifier = new("Const");
            ToolStripMenuItem verticalmodifier = new("Vertical");
            ToolStripMenuItem lock45modifier = new("Lock45");
            normalmodifier.Click += (_, __) => { if (Hit != null) ApplyModifierNormal(); Hit = null; };
            constmodifier.Click += (_, __) => { if (Hit != null) ApplyModifierConst(); Hit = null; };
            verticalmodifier.Click += (_, __) => { if (Hit != null) ApplyModifierVertical(); Hit = null; };
            lock45modifier.Click += (_, __) => { if (Hit != null) ApplyModifierLock45(); Hit = null; };
            modifiers.DropDownItems.Add(normalmodifier);
            modifiers.DropDownItems.Add(constmodifier);
            modifiers.DropDownItems.Add(verticalmodifier);
            modifiers.DropDownItems.Add(lock45modifier);
            Menu.Items.Add(modifiers);

            Owner.MouseDown += Owner_MouseDown;
        }
        private void Owner_MouseDown(object? s, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            Point p = e.Location;
            Hit = HitTestEdge(p, 6);
            if (Hit != null) Menu.Show(Owner, p);
        }
        private Edge? HitTestEdge(Point p, int thresh)
        {
            foreach (Edge edge in Owner.Edges)
            {
                Point a = edge.V1.Center();
                Point b = edge.V2.Center();
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
            ConstForm constForm = new();
            constForm.NUP1.Value = Hit!.CurrLength();
            if (constForm.ShowDialog() == DialogResult.Cancel) return;
            double desiredLength = (double)constForm.NUP1.Value;
            var c1 = Hit!.V1.Center();
            var c2 = Hit!.V2.Center();
            double vx = c2.X - c1.X;
            double vy = c2.Y - c1.Y;
            double dist = Math.Sqrt(vx * vx + vy * vy);
            double ux = 1.0, uy = 0.0;
            if (dist > 1e-6)
            {
                ux = vx / dist;
                uy = vy / dist;
            }
            double newCenterX = c1.X + ux * desiredLength;
            double newCenterY = c1.Y + uy * desiredLength;
            int newLocX = (int)Math.Round(newCenterX - Hit!.V2.Width / 2.0);
            int newLocY = (int)Math.Round(newCenterY - Hit!.V2.Height / 2.0);
            Point newLocation = new(newLocX, newLocY);
            Hit!.V2.Location = newLocation;
            Hit!.Modifier = EdgeModifier.Const;
            Hit!.ConstLength = Hit!.CurrLength();
            ModifierMove(newLocation, Hit!.V2);
            Owner.Invalidate();
        }
        private void ApplyModifierVertical()
        {
            if (Hit != null && Hit.Modifier == EdgeModifier.Vertical) return;
            bool b = false;
            if (Hit != null && (Hit.V1.LeftEdge!.Modifier == EdgeModifier.Vertical || Hit.V1.RightEdge!.Modifier == EdgeModifier.Vertical)) b = true;
            if (Hit != null && (Hit.V2.LeftEdge!.Modifier == EdgeModifier.Vertical || Hit.V2.RightEdge!.Modifier == EdgeModifier.Vertical)) b = true;

            if (b)
            {
                MessageBox.Show("Adjacent edge is vertical!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Hit!.Modifier = EdgeModifier.Vertical;
            Hit!.ConstLength = 0;

            Point newLocation = new(Hit!.V1.Location.X, Hit!.V1.Location.Y + Hit!.CurrLength());
            Hit!.V2.Location = newLocation;
            ModifierMove(newLocation, Hit!.V2);

            Owner.Invalidate();
        }
        private void ApplyModifierLock45()
        {
            Hit!.Modifier = EdgeModifier.Lock45;
            Hit!.ConstLength = 0;

            int a = (int)(Hit!.CurrLength() / Math.Sqrt(2));
            Point newLocation = new(Hit!.V1.Location.X + a, Hit!.V1.Location.Y - a);
            Hit!.V2.Location = newLocation;
            ModifierMove(newLocation, Hit!.V2);

            Owner.Invalidate();
        }
        private void ModifierMove(Point mouse, Vertex dragged)
        {
            (List<Vertex> leftchain, List<Vertex> rightchain) = Owner.GetMoveChain(dragged);
            if (leftchain.Count == 0 && rightchain.Count == 0)
            {
                Point newLocation = new(mouse.X - Owner.MouseOffset.X, mouse.Y - Owner.MouseOffset.Y);
                dragged.Location = newLocation;
                Owner.Invalidate();
            }
            else
            {
                Point newLocation = new(mouse.X - Owner.MouseOffset.X, mouse.Y - Owner.MouseOffset.Y);
                dragged.Location = newLocation;

                Vertex v = dragged;
                for (int i = 0; i < rightchain.Count; i++)
                {
                    Owner.ApplyMove(v, rightchain[i]);
                    v = rightchain[i];
                }
                v = dragged;
                for (int i = 0; i < leftchain.Count; i++)
                {
                    Owner.ApplyMove(v, leftchain[i]);
                    v = leftchain[i];
                }
                Owner.Invalidate();
            }
        }
    }
}