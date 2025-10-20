using Grafika.Constraints;
using Grafika.Controls;
using Grafika.Elements;
using Grafika.ExtraConstraints;
using Grafika.Visuals;

namespace Grafika.ExtraCommands
{
    internal class EdgeRightClickMenu
    {
        public Polygon Polygon { get; set; }
        public ContextMenuStrip Menu { get; set; }
        public Edge? Hit { get; set; } = null;
        public EdgeRightClickMenu(Polygon polygon)
        {
            Polygon = polygon;
            Menu = new();

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

            Polygon.MouseDown += Owner_MouseDown;
        }
        public void Owner_MouseDown(object? s, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            Point p = e.Location;
            Hit = HitTestEdge(p, 6);
            if (Hit != null) Menu.Show(Polygon, p);
        }
        public Edge? HitTestEdge(Point p, int thresh)
        {
            foreach (Edge edge in Polygon.Edges)
            {
                Point a = edge.V1.Center();
                Point b = edge.V2.Center();
                if (DistancePointToSegment(p, a, b) <= thresh) return edge;
            }
            return null;
        }
        public static double DistancePointToSegment(Point p, Point a, Point b)
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
        public void AddVertex()
        {
            Polygon.TCommands.RemoveBezierSegment(Hit!);
            Polygon.TCommands.AddVertexOnMiddle(Hit!);
            Hit = null;
            Polygon.Invalidate();
        }
        private void ApplyTypeNormal()
        {
            Hit!.Visual = new Bresenham();
            Polygon.TCommands.RemoveBezierSegment(Hit!);
            Polygon.Invalidate();
        }
        private void ApplyTypeSemiCircle()
        {
            if (Hit!.Constraint is not NoConstraint)
            {
                MessageBox.Show("Cannot aply semicircle to constraint edge!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Hit!.Visual = new SemiCircle();
            Polygon.TCommands.RemoveBezierSegment(Hit!);
            Polygon.Invalidate();
        }
        private void ApplyTypeBezier()
        {
            if (Hit!.Constraint is not NoConstraint)
            {
                MessageBox.Show("Cannot aply Bezier to constraint edge!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Edge e1;
            Edge e2;
            if (Hit!.V1.LeftEdge == Hit) e1 = Hit!.V1.RightEdge!;
            else e1 = Hit!.V1.LeftEdge!;
            if (Hit!.V2.LeftEdge == Hit) e2 = Hit!.V2.RightEdge!;
            else e2 = Hit!.V2.LeftEdge!;
            if (e1.Visual is Bezier || e2.Visual is Bezier)
            {
                MessageBox.Show("Adjecent edge is Bezier type!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Hit!.Visual = new Bezier();
            Polygon.TCommands.AddBezierSegment(Hit!);
            RepairG1.Repair(Polygon.Segments);
            RepairC1.Repair(Polygon.Segments);
            Polygon.Invalidate();
        }
        public void ApplyModifierNormal()
        {
            Hit!.ConstLength = 0;
            Hit!.Constraint = new NoConstraint();
            Hit!.G1BezierConstraint = new G1BezierNoConstraint();
            Hit!.C1BezierConstraint = new C1BezierNoConstraint();
            Hit!.Label = new NormalLabel();
            Polygon.Invalidate();
        }
        public void ApplyModifierConst()
        {
            if (Hit!.Visual is not Bresenham)
            {
                MessageBox.Show("Cannot aply constraint to Bezier type edge!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ConstForm constForm = new();
            constForm.NUP1.Value = Hit!.CurrLength();
            if (constForm.ShowDialog() == DialogResult.Cancel) return;

            bool b = true;
            double max = 0;
            foreach (Edge e in Polygon.Edges)
            {
                if (e.Constraint is ConstConstraint)
                {
                    max += e.ConstLength;
                }
                else if (e.Constraint is not ConstConstraint && e != Hit!)
                {
                    b = false;
                    break;
                }
            }
            if (b)
            {
                if (max < (double)constForm.NUP1.Value)
                {
                    MessageBox.Show("Cannot apply - too long!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

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

            Hit!.ConstLength = Hit!.CurrLength();
            Hit!.Constraint = new ConstConstraint();
            Hit!.G1BezierConstraint = new G1BezierConst();
            Hit!.C1BezierConstraint = new C1BezierConst();
            Hit!.Label = new ConstLabel();

            Move(Hit!.V2);
            RepairG1.Repair(Polygon.Segments);
            RepairC1.Repair(Polygon.Segments);

            Polygon.Invalidate();
        }
        public void ApplyModifierVertical()
        {
            if (Hit!.Visual is not Bresenham)
                {
                MessageBox.Show("Cannot aply constraint to Bezier type edge!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool b = false;
            if (Hit != null && (Hit.V1.LeftEdge!.Constraint is VerticalConstraint || Hit.V1.RightEdge!.Constraint is VerticalConstraint)) b = true;
            if (Hit != null && (Hit.V2.LeftEdge!.Constraint is VerticalConstraint || Hit.V2.RightEdge!.Constraint is VerticalConstraint)) b = true;

            if (b)
            {
                MessageBox.Show("Adjacent edge is vertical!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Point newLocation = new(Hit!.V1.Location.X, Hit!.V1.Location.Y + Hit!.CurrLength());
            Hit!.V2.Location = newLocation;

            Hit!.ConstLength = 0;
            Hit!.Constraint = new VerticalConstraint();
            Hit!.G1BezierConstraint = new G1BezierVertical();
            Hit!.C1BezierConstraint = new C1BezierVertical();
            Hit!.Label = new VerticalLabel();

            Move(Hit!.V2);
            RepairG1.Repair(Polygon.Segments);
            RepairC1.Repair(Polygon.Segments);

            Polygon.Invalidate();
        }
        public void ApplyModifierLock45()
        {
            if (Hit!.Visual is not Bresenham)
            {
                MessageBox.Show("Cannot aply constraint to Bezier type edge!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int a = (int)(Hit!.CurrLength() / Math.Sqrt(2));

            Point newLocation = new(Hit!.V1.Location.X + a, Hit!.V1.Location.Y - a);
            Hit!.V2.Location = newLocation;

            Hit!.ConstLength = 0;
            Hit!.Constraint = new Lock45Constraint();
            Hit!.G1BezierConstraint = new G1BezierLock45();
            Hit!.C1BezierConstraint = new C1BezierLock45();
            Hit!.Label = new Lock45Label();

            Move(Hit!.V2);
            RepairG1.Repair(Polygon.Segments);
            RepairC1.Repair(Polygon.Segments);

            Polygon.Invalidate();
        }
        public static void Move(Vertex dragged)
        {
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
    }
}