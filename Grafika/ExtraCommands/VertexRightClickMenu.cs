using Grafika.Controls;

namespace Grafika.ExtraCommands
{
    internal class VertexRightClickMenu
    {
        public Vertex Vertex { get; set; }
        public ContextMenuStrip Menu { get; set; }
        public VertexRightClickMenu(Vertex vertex)
        {
            Vertex = vertex;
            Menu = new();

            ToolStripMenuItem remove = new("Remove vertex");
            remove.Click += (_, __) =>
            {
                if (Vertex.Parent is Polygon polygon)
                {
                    polygon.TCommands.RemoveVertexOnMiddle(Vertex);
                    polygon.Invalidate();
                }
            };
            Menu.Items.Add(remove);

            ToolStripMenuItem modifiers = new("Modifiers");
            ToolStripMenuItem normalmodifier = new("Normal");
            ToolStripMenuItem g1modifier = new("G1");
            ToolStripMenuItem c1modifier = new("C1");
            normalmodifier.Click += (_, __) =>
            {
                if (Vertex.Parent is Polygon polygon)
                {
                    Vertex.Type = VertexType.G0;
                    polygon.Invalidate();
                }
            };
            g1modifier.Click += (_, __) =>
            {
                if (Vertex.Parent is Polygon polygon)
                {
                    Vertex v1 = Vertex.LeftEdge!.V1;
                    if (v1 == Vertex) v1 = Vertex.LeftEdge!.V2;
                    Vertex v2 = Vertex.RightEdge!.V1;
                    if (v2 == Vertex) v2 = Vertex.RightEdge!.V2;

                    if (v1.Type == VertexType.G1 || v2.Type == VertexType.G1)
                    {
                        MessageBox.Show("Adjacent vertex is G1!", "404", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    Vertex.Type = VertexType.G1;

                    polygon.Invalidate();
                }
            };
            c1modifier.Click += (_, __) => { };
            modifiers.DropDownItems.Add(normalmodifier);
            modifiers.DropDownItems.Add(g1modifier);
            modifiers.DropDownItems.Add(c1modifier);
            Menu.Items.Add(modifiers);
        }
    }
}