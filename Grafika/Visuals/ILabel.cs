using Grafika.Elements;

namespace Grafika.Visuals
{
    internal interface ILabel
    {
        string Label(Edge e);
        public static void DrawEdgeLabel(Graphics g, Edge e, string text)
        {
            if (text == null || text.Length == 0 || text == "") return;

            var font = SystemFonts.DefaultFont;
            var a = e.V1.Center();
            var b = e.V2.Center();

            float mx = (a.X + b.X) / 2f;
            float my = (a.Y + b.Y) / 2f;

            SizeF sz = g.MeasureString(text, font);

            RectangleF bg = new(mx - sz.Width / 2f - 4f, my - sz.Height / 2f - 2f, sz.Width + 8f, sz.Height + 4f);

            using var brushBg = new SolidBrush(Color.FromArgb(220, Color.White));
            using var penBorder = new Pen(Color.Black, 1f);
            using var brushText = new SolidBrush(Color.Black);

            g.FillRectangle(brushBg, bg);
            g.DrawRectangle(penBorder, Rectangle.Round(bg));

            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.DrawString(text, font, brushText, bg, sf);
        }
    }
    internal class NormalLabel : ILabel
    {
        public string Label(Edge e)
        {
            return "";
        }
    }
    internal class ConstLabel : ILabel
    {
        public string Label(Edge e)
        {
            return e.ConstLength.ToString();
        }
    }
    internal class VerticalLabel : ILabel
    {
        public string Label(Edge e)
        {
            return "Vertical";
        }
    }
    internal class Lock45Label : ILabel
    {
        public string Label(Edge e)
        {
            return "Lock45";
        }
    }
}