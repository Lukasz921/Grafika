using Proj1.Components;

namespace Proj1
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
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string instrukcja = "Projekt 1 � �ukasz Przybylski\n\n" +
                "� Przesuwanie wierzcho�ka lewym przyciskiem myszy\n" +
                "� Przesuwanie ca�ego grafu lewym przyciskiem myszy + wci�ni�cie CTRL\n" +
                "� Otwarcie menu prawym przyciskiem myszy (odpowiednio na wierzcho�ku lub kraw�dzi)\n\n" +
                "� Menu dla kraw�dzi:\n" +
                "� Add Vertex: dodaje wierzcho�ek na �rodku\n" +
                "� Types:\n" +
                "      - Normal (kraw�d� klasyczna)\n" +
                "      - Semicircle (p�okr�g zbudowany na kraw�dzi)\n" +
                "      - Bezier (fragment segmentu Beziera)\n" +
                "� Modifiers:\n" +
                "      - Normal (cofni�cie ograniczenia)\n" +
                "      - Const (kraw�d� o sta�ej d�ugo�ci)\n" +
                "      - Vertical (kraw�d� pionowa)\n" +
                "      - Lock45 (kraw�d� ustawiona pod k�tem 45 stopni)\n\n" +
                "� Menu dla wierzcho�ka:\n" +
                "� Remove Vertex: usuwa wierzcho�ek\n" +
                "� Modifiers:\n" +
                "      - Normal (G0) (cofni�cie ograniczenia)\n" +
                "      - G1 (ograniczenie G1 dla p�okr�gu)\n" +
                "      - C1 (ograniczenie C1 dla segmentu Beziera)";
            MessageBox.Show(instrukcja, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}