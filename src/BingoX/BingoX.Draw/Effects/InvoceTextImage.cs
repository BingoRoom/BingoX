using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace BingoX.Draw.Effects
{
    public class InvoceTextImage : ImageBuilder
    {
        public InvoceTextImage(Size size, string invcCode, string invcSize)
        {
            Size = size;
            InvcCode = invcCode;
            InvcSize = invcSize;
        }

        public Size Size { get; private set; }
        public string InvcCode { get; private set; }
        public string InvcSize { get; private set; }

        public override Image ProcessBitmap()
        {
            Bitmap bitmap = new Bitmap(Size.Width, Size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);
            g.DrawString(InvcCode, DrawPrivateFontFamily.CreateFont(0, 16, FontStyle.Regular, GraphicsUnit.World), Brushes.Black, 15, 40);

            g.DrawString(InvcSize, DrawPrivateFontFamily.CreateFont(0, 20, FontStyle.Regular, GraphicsUnit.World), Brushes.Red, 15, 80);
            g.Dispose();
            return bitmap;
        }
    }

}
