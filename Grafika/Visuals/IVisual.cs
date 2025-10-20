using Grafika.Controls;
using Grafika.Elements;

namespace Grafika.Visuals
{
    internal interface IVisual
    {
        void DrawEdge(Graphics g, Edge e, SolidBrush brush, Pen pen1, Polygon polygon);
    }
}