using Grafika.Controls;
using Grafika.Elements;

namespace Grafika
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Polygon polygon = new()
            {
                Dock = DockStyle.Fill,
            };
            Controls.Add(polygon);

            Vertex v1 = new(new(100, 100));
            Vertex v2 = new(new(200, 100));
            Vertex v3 = new(new(200, 200));
            Vertex v4 = new(new(100, 200));

            Edge a = new(v1, v2);
            Edge b = new(v2, v3);
            Edge c = new(v3, v4);
            Edge d = new(v4, v1);

            v1.LeftEdge = d;
            v1.RightEdge = a;
            v2.LeftEdge = a;
            v2.RightEdge = b;
            v3.LeftEdge = b;
            v3.RightEdge = c;
            v4.LeftEdge = c;
            v4.RightEdge = d;

            polygon.Vertices.Add(v1);
            polygon.Controls.Add(v1);
            polygon.Vertices.Add(v2);
            polygon.Controls.Add(v2);
            polygon.Vertices.Add(v3);
            polygon.Controls.Add(v3);
            polygon.Vertices.Add(v4);
            polygon.Controls.Add(v4);
            polygon.Edges.Add(a);
            polygon.Edges.Add(b);
            polygon.Edges.Add(c);
            polygon.Edges.Add(d);
        }
    }
}