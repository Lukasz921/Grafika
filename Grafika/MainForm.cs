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
            Vertex v3 = new(new(300, 100));
            Vertex v4 = new(new(300, 200));
            Vertex v5 = new(new(200, 200));
            Vertex v6 = new(new(100, 200));

            Edge a = new(v1, v2);
            Edge b = new(v2, v3);
            Edge c = new(v3, v4);
            Edge d = new(v4, v5);
            Edge e = new(v5, v6);
            Edge f = new(v6, v1);

            v1.LeftEdge = f;
            v1.RightEdge = a;
            v2.LeftEdge = a;
            v2.RightEdge = b;
            v3.LeftEdge = b;
            v3.RightEdge = c;
            v4.LeftEdge = c;
            v4.RightEdge = d;
            v5.LeftEdge = d;
            v5.RightEdge = e;
            v6.LeftEdge = e;
            v6.RightEdge = f;

            polygon.Vertices.Add(v1);
            polygon.Controls.Add(v1);
            polygon.Vertices.Add(v2);
            polygon.Controls.Add(v2);
            polygon.Vertices.Add(v3);
            polygon.Controls.Add(v3);
            polygon.Vertices.Add(v4);
            polygon.Controls.Add(v4);
            polygon.Vertices.Add(v5);
            polygon.Controls.Add(v5);
            polygon.Vertices.Add(v6);
            polygon.Controls.Add(v6);
            polygon.Edges.Add(a);
            polygon.Edges.Add(b);
            polygon.Edges.Add(c);
            polygon.Edges.Add(d);
            polygon.Edges.Add(e);
            polygon.Edges.Add(f);

            
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "Projekt 1 £ukasz Przybylski - Instrukcja obs³ugi\n" +
                         "Wierzcho³ek / segment Beziera da siê przeci¹gn¹æ LPM\n" +
                         "Aby przenieœæ wszystkie jednoczeœnie, nale¿y wcisn¹æ lewy CTRL i LPM\n\n" +
                         "Naciœniêcie PPM na krawêdŸ wyœwietla menu:\n" +
                         "- Add Vertex (dodaje nowy wierzcho³ek na œrodku danej krawêdzi)\n" +
                         "- Types (typy krawêdzi)\n" +
                         "  - Normal (normalna krawêdŸ - algorytm Bresenhama)\n" +
                         "  - Semicircle (pó³okr¹g - algorytm biblioteczny)\n" +
                         "  - Bezier (tworzy segment Beziera na zadanej krawêdzi)\n" +
                         "- Modifiers (ograniczenia na krawêdŸ)\n" +
                         "  - Normal (brak ograniczeñ)\n" +
                         "  - Const (sta³a d³ugoœæ krawêdzi)\n" +
                         "  - Vertical (krawêdŸ pionowa)\n" +
                         "  - Lock45 (krawêdŸ zablokowana pod k¹tem 45 stopni)\n\n" +
                         "Naciœniêcie PPM na wierzcho³ek wyœwietla menu:\n" +
                         "- Remove Vertex (usuwa zadany wierzcho³ek)\n" +
                         "- Modifiers (ograniczenia na wierzcho³ek)\n" +
                         "  - Normal (brak ograniczeñ)\n" +
                         "  - G1 (ci¹g³oœæ G1)\n" +
                         "  - C1 (ci¹g³oœæ C1)\n\n" +
                         "Oznaczenia kolorów:\n" +
                         "  - czerwony (zwyk³y wierzcho³ek)\n" +
                         "  - niebieski (segment Beziera)\n" +
                         "  - zielony (ci¹g³oœæ G1)\n" +
                         "  - fioletowy (ci¹g³oœæ C1)\n";
            MessageBox.Show(str, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}