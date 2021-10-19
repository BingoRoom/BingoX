using BingoX.Helper;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace BingoX.Draw
{
    public static class DrawPrivateFontFamily
    {
        static DrawPrivateFontFamily()
        {
            var stream = typeof(DrawPrivateFontFamily).Assembly.GetManifestResourceStream("BingoX.Draw.simkai.ttf");

            AddMemoryFont(stream.ToArray());
        }
        readonly static PrivateFontCollection FontCollection = new PrivateFontCollection();

        public static void AddFontFile(string path)
        {
            FontCollection.AddFontFile(path);
        }
        public static void AddMemoryFont(byte[] buffer)
        {
            var handle = System.Runtime.InteropServices.GCHandle.Alloc(buffer, System.Runtime.InteropServices.GCHandleType.Pinned);

            try
            {
                var ptr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);

                FontCollection.AddMemoryFont(ptr, buffer.Length);

            }
            finally
            {
                handle.Free();
            }
        }
        public static Font CreateFont(int index, float emSize, FontStyle style = FontStyle.Bold, GraphicsUnit unit = GraphicsUnit.Pixel)
        {
            return new Font(FontCollection.Families[index], emSize, style, unit);
        }
        public static Font CreateFont(string familyName, float emSize, FontStyle style = FontStyle.Bold, GraphicsUnit unit = GraphicsUnit.Pixel)
        {
            var families = FontCollection.Families.FirstOrDefault(n => n.Name == familyName);
            return new Font(families, emSize, style, unit);
        }
    }
}
namespace BingoX.Draw.Effects
{

    public class SignetImage : ImageBuilder
    {

        public string Text { get; set; }

        //  readonly static FontFamily fontFamily = new FontFamily("楷体");
        readonly static Color Red = Color.FromArgb(80, 255, 0, 0);
        readonly static SolidBrush RedBrush = new SolidBrush(Red);
        readonly static SolidBrush RedBrush2 = new SolidBrush(Color.FromArgb(100, 200, 0, 0));
        public bool AutoRotate { get; set; }
        public override Image ProcessBitmap()
        {


            Image Cover = Source.Clone() as Image;
            if (AutoRotate && Cover.Height > Cover.Width)
            {
                var stream = new System.IO.MemoryStream();
                Cover.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                Cover.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.TrySeek();
                Cover = Image.FromStream(stream);

            }
            try
            {
                int angle = -10;


                Graphics g = Graphics.FromImage(Cover);
                //  g.TranslateTransform(Cover.Width / 2, Cover.Height / 2);

                var rectangle = new Rectangle(Cover.Width / 2 - 100, Cover.Height / 2 - 50, Cover.Width / 3, Cover.Width / 10);

                var font = DrawPrivateFontFamily.CreateFont(0, rectangle.Width / (Text.Length + 3));
                StringFormat stringFormat = StringFormat.GenericDefault;
                //StringFormat sf = StringFormat.GenericTypographic;
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Near;
                var stringSize = g.MeasureString(Text, font, new SizeF(rectangle.Width * 0.75F, rectangle.Height), stringFormat);

                Matrix matrix = g.Transform;
                matrix.RotateAt(angle, rectangle.Location);
                g.Transform = matrix;
                RectangleF descRect = new RectangleF(rectangle.Location, stringSize + new SizeF(10, 0));
                g.DrawRectangles(new Pen(Red, 2), new RectangleF[] { descRect, descRect.Add(10) });
                descRect.Location = descRect.Location + new SizeF(5, (rectangle.Height - stringSize.Height) / 2.5F);

                g.DrawString(Text, font, RedBrush2, descRect, stringFormat);
                descRect.Location = descRect.Location + new SizeF(-3, -2);
                g.DrawString(Text, font, RedBrush, descRect, stringFormat);



                g.Dispose();


                return Cover;

            }
            finally
            {


            }
        }
    }

}
