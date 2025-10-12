using WinFormsApp1.Elementy;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly GraphPanel graphPanel;
        public Form1()
        {
            InitializeComponent();
            graphPanel = new GraphPanel { Dock = DockStyle.Fill };
            Controls.Add(graphPanel);

            Vertex v1 = new() { Location = new Point(250, 250) };
            Vertex v2 = new() { Location = new Point(500, 250) };
            Vertex v3 = new() { Location = new Point(500, 500) };
            Vertex v4 = new() { Location = new Point(250, 500) };

            graphPanel.AddVertex(v1);
            graphPanel.AddVertex(v2);
            graphPanel.AddVertex(v3);
            graphPanel.AddVertex(v4);

            graphPanel.AddEdge(v1, v2);
            graphPanel.AddEdge(v2, v3);
            graphPanel.AddEdge(v3, v4);
            graphPanel.AddEdge(v4, v1);

            NUD1.ValueChanged += NUD1_ValueChanged!;

            NUD1.Minimum = 0;
            NUD1.Maximum = graphPanel.Edges.Count - 1;
            NUD2.Minimum = 0;
            NUD2.Maximum = 10000;
            graphPanel.SelectedEdge = graphPanel.Edges[0];
            NUD2.Value = (int)graphPanel.SelectedEdge.Length();
            graphPanel.Invalidate();
        }
        private void NUD1_ValueChanged(object sender, EventArgs e)
        {
            if (graphPanel == null) return;
            int idx = (int)NUD1.Value;
            if (idx >= 0 && idx < graphPanel.Edges.Count)
            {
                graphPanel.SelectedEdge = graphPanel.Edges[idx];
                NUD2.Value = (int)graphPanel.SelectedEdge.Length();
            }
            else
            {
                graphPanel.SelectedEdge = null;
            }
            graphPanel.Invalidate();
        }
        private void B1_Click(object sender, EventArgs e)
        {
            if (graphPanel == null) return;
            var sel = graphPanel.SelectedEdge;
            if (sel == null) return;
            graphPanel.AddVertexOnEdge(sel);
            if (graphPanel.Edges.Count > 0)
            {
                NUD1.Maximum = Math.Max(0, graphPanel.Edges.Count - 1);
                NUD1.Value = 0;
            }
            else
            {
                NUD1.Maximum = 0;
                NUD1.Value = 0;
                graphPanel.SelectedEdge = null;
            }
            graphPanel.Invalidate();
        }
        private void B2_Click(object sender, EventArgs e)
        {
            if (graphPanel == null || graphPanel.SelectedEdge == null) return;
            graphPanel.SelectedEdge.Type = EdgeType.Normal;
            NUD2.Value = (int)graphPanel.SelectedEdge!.Length();
            graphPanel.Invalidate();
        }
        private void B3_Click(object sender, EventArgs e)
        {
            if (graphPanel == null || graphPanel.SelectedEdge == null) return;
            graphPanel.SelectedEdge.Type = EdgeType.SemiCircle;
            graphPanel.Invalidate();
        }
        private void B4_Click(object sender, EventArgs e)
        {
            if (graphPanel == null || graphPanel.SelectedEdge == null) return;
            Edge edge = graphPanel.SelectedEdge;
            edge.Modifier = EdgeModifier.Vertical;
            double height = edge.Length();
            int newY = edge.A.Center.Y - (int)height;
            if (newY < 0) newY = 0;
            edge.B.Location = new Point(edge.A.Center.X - (edge.A.SizE / 2), newY - (edge.A.SizE / 2));
            NUD2.Value = (int)graphPanel.SelectedEdge!.Length();
            graphPanel.Invalidate();
        }

        private void B5_Click(object sender, EventArgs e)
        {
            if (graphPanel == null || graphPanel.SelectedEdge == null) return;
            Edge edge = graphPanel.SelectedEdge;
            edge.Modifier = EdgeModifier.Lock45;
            double len = edge.Length();
            len /= Math.Sqrt(2);
            edge.B.Location = new Point(edge.A.Center.X + (int)len - (edge.A.SizE / 2), edge.A.Center.Y - (int)len - (edge.A.SizE / 2));
            NUD2.Value = (int)graphPanel.SelectedEdge!.Length();
            graphPanel.Invalidate();
        }
        private void B6_Click(object sender, EventArgs e)
        {
            if (graphPanel == null || graphPanel.SelectedEdge == null) return;
            graphPanel.SelectedEdge.Modifier = EdgeModifier.Normal;
            graphPanel.Invalidate();
        }

        private void B7_Click(object sender, EventArgs e)
        {
            if (graphPanel == null || graphPanel.SelectedEdge == null) return;
            graphPanel.SelectedEdge.Modifier = EdgeModifier.Const;
            graphPanel.Invalidate();
        }
    }
}