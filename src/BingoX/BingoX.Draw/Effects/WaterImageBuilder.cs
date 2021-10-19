using System;
using System.Drawing;
using System.IO;

namespace BingoX.Draw.Effects
{
    /// <summary>
    ///
    /// </summary>
    public class WaterImageBuilderByPixel : WaterImageBuilder
    {
        private Localization _localization = Localization.BottomRight;

        /// <summary>
        ///
        /// </summary>
        public Localization Localization
        {
            get { return _localization; }
            set { _localization = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceImg"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected override Rectangle GetWaterRectangle(Image sourceImg, Image waterImg)
        {
            Rectangle waterRectangle;
            var subtractx = sourceImg.Width - waterImg.Width;
            var subtracty = sourceImg.Height - waterImg.Height;
            switch (_localization)
            {
                case Localization.Top:
                    waterRectangle = new Rectangle(subtractx / 2, 0, waterImg.Width, waterImg.Height);
                    break;

                case Localization.TopLeft:
                    waterRectangle = new Rectangle(0, 0, waterImg.Width, waterImg.Height);
                    break;

                case Localization.TopRight:
                    waterRectangle = new Rectangle(subtractx, 0, waterImg.Width, waterImg.Height);
                    break;

                case Localization.Centre:
                    waterRectangle = new Rectangle(subtractx / 2, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;

                case Localization.CentreLeft:
                    waterRectangle = new Rectangle(0, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;

                case Localization.CentreRight:
                    waterRectangle = new Rectangle(subtractx, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;

                case Localization.Bottom:
                    waterRectangle = new Rectangle(subtractx / 2, subtracty, waterImg.Width, waterImg.Height);
                    break;

                case Localization.BottomLeft:
                    waterRectangle = new Rectangle(0, subtracty, waterImg.Width, waterImg.Height);
                    break;
                //case Localization.BottomRight:
                default:
                    waterRectangle = new Rectangle(subtractx, subtracty, waterImg.Width, waterImg.Height);
                    break;
            }
            return waterRectangle;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected override Image CreateFillImage(Rectangle rectangle, Image waterImg)
        {
            return waterImg;
        }
    }
    public class WaterImageBuilderByTile : WaterImageBuilder
    {
        private WaterImageTileOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is WaterImageTileOption == false) throw new ImageException("Opetion is not WaterImageTileOption");
                _opetion = value as WaterImageTileOption;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="space"></param>
        public void SetSpace(Size space)
        {
            InitOption();
            _opetion.Space = space;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offset"></param>
        public void SetOffset(Point offset)
        {
            InitOption();
            _opetion.Offset = offset;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new WaterImageTileOption();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected override Image CreateFillImage(Rectangle rectangle, Image waterImg)
        {
            Image img = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics gType = Graphics.FromImage(img);

            int spacey = _opetion.Space != null ? _opetion.Space.Value.Height : waterImg.Height;
            int spaceX = _opetion.Space != null ? _opetion.Space.Value.Width : waterImg.Width;
            int offsetY = _opetion.Offset.GetValueOrDefault().Y;
            int offsetX = _opetion.Offset.GetValueOrDefault().X;

            var clo = rectangle.Width / (waterImg.Width + spaceX);
            var row = rectangle.Height / (waterImg.Height + spacey);
            for (int r = -2; r <= row; r++)
            {
                for (int c = -2; c <= clo; c++)
                {
                    int y = (waterImg.Height + spacey) * r + (offsetY * c);
                    int x = (waterImg.Width + spaceX) * c + (offsetX * r);
                    gType.DrawImage(waterImg, new Rectangle(x, y, waterImg.Width, waterImg.Width), 0, 0, waterImg.Width, waterImg.Height, GraphicsUnit.Pixel);
                }
            }

            return img;
        }
    }
    /// <summary>
    ///
    /// </summary>
    public class WaterImageBuilderByText : WaterImageBuilder
    {
        private Localization _localization = Localization.BottomRight;

        /// <summary>
        ///
        /// </summary>
        public Localization Localization
        {
            get { return _localization; }
            set { _localization = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Font TextFont { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            if (Opetion == null) throw new ImageException("Opetion is null");
            if (TextFont == null) TextFont = new Font("SimSun", 20);
            MemoryStream sourcestream = new MemoryStream(SourceImgBuffter);

            var sourceImg = Image.FromStream(sourcestream);

            var trageSize = Opetion == null ? null : Opetion.TragetSize;
            Image tmpimg = trageSize != null ? new Bitmap(trageSize.Value.Width, trageSize.Value.Height) : new Bitmap(sourceImg.Width, sourceImg.Height);
            Graphics gType = CreateGraphics(tmpimg, sourceImg);
            var attributes = GetOpacity(Opetion.Opacity);
            var sizef = gType.MeasureString(Text, TextFont, sourceImg.Width, StringFormat.GenericDefault);
            var waterImg = new Bitmap((int)sizef.Width, (int)sizef.Height);
            var waterRectangle = GetWaterRectangle(sourceImg, waterImg);
            var tmpwatrer = CreateFillImage(waterImg);
            try
            {
                gType.DrawImage(tmpwatrer, waterRectangle, 0, 0, tmpwatrer.Width, tmpwatrer.Height, GraphicsUnit.Pixel, attributes);
                return tmpimg;
            }
            finally
            {
                sourcestream.Dispose();
                tmpwatrer.Dispose();
                sourceImg.Dispose();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceImg"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected override Rectangle GetWaterRectangle(Image sourceImg, Image waterImg)
        {
            Rectangle waterRectangle;
            var subtractx = sourceImg.Width - waterImg.Width;
            var subtracty = sourceImg.Height - waterImg.Height;
            switch (_localization)
            {
                case Localization.Top:
                    waterRectangle = new Rectangle(subtractx / 2, 0, waterImg.Width, waterImg.Height);
                    break;

                case Localization.TopLeft:
                    waterRectangle = new Rectangle(0, 0, waterImg.Width, waterImg.Height);
                    break;

                case Localization.TopRight:
                    waterRectangle = new Rectangle(subtractx, 0, waterImg.Width, waterImg.Height);
                    break;

                case Localization.Centre:
                    waterRectangle = new Rectangle(subtractx / 2, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;

                case Localization.CentreLeft:
                    waterRectangle = new Rectangle(0, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;

                case Localization.CentreRight:
                    waterRectangle = new Rectangle(subtractx, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;

                case Localization.Bottom:
                    waterRectangle = new Rectangle(subtractx / 2, subtracty, waterImg.Width, waterImg.Height);
                    break;

                case Localization.BottomLeft:
                    waterRectangle = new Rectangle(0, subtracty, waterImg.Width, waterImg.Height);
                    break;
                //case Localization.BottomRight:
                default:
                    waterRectangle = new Rectangle(subtractx, subtracty, waterImg.Width, waterImg.Height);
                    break;
            }
            return waterRectangle;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected Image CreateFillImage(Image waterImg)
        {
            Graphics gType = Graphics.FromImage(waterImg);
            gType.DrawString(Text, TextFont, new SolidBrush(Color.Black), 1, 1);
            gType.DrawString(Text, TextFont, new SolidBrush(Color.White), 0, 0);
            return waterImg;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected override Image CreateFillImage(Rectangle rectangle, Image waterImg)
        {
            return CreateFillImage(waterImg);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class WaterImageBuilderByFill : WaterImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected override Image CreateFillImage(Rectangle rectangle, Image waterImg)
        {
            Image img = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics gType = Graphics.FromImage(img);

            gType.DrawImage(waterImg, rectangle, 0, 0, waterImg.Width, waterImg.Height, GraphicsUnit.Pixel);

            return img;
        }
    }
    /// <summary>
    ///
    /// </summary>
    public class WaterImageFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="waterImageType"></param>
        /// <returns></returns>
        public static WaterImageBuilder CreateBuilder(WaterImageType waterImageType)
        {
            WaterImageBuilder builder = null;
            switch (waterImageType)
            {
                case WaterImageType.Full: builder = new WaterImageBuilderByFill(); break;
                case WaterImageType.Pixel: builder = new WaterImageBuilderByPixel(); break;
                case WaterImageType.Tile: builder = new WaterImageBuilderByTile(); break;
                case WaterImageType.Text: builder = new WaterImageBuilderByText(); break;
            }
            return builder;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="waterImageType"></param>
        /// <param name="sourceImgPath"></param>
        /// <param name="waterImgPath"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Image CreateWaterImage(WaterImageType waterImageType, string sourceImgPath, string waterImgPath, ImageOption option)
        {
            WaterImageBuilder builder = new WaterImageBuilderByFill();

            builder.SetSourceImage(sourceImgPath);
            builder.SetWaterImage(waterImgPath);
            builder.SetOpetion(option);
            return builder.ProcessBitmap();
        }
    }
    public sealed class WaterImageTileOption : ImageOption
    {
        /// <summary>
        /// 偏移
        /// </summary>
        public Point? Offset { get; set; }

        /// <summary>
        /// 隔多少空間
        /// </summary>
        public Size? Space { get; set; }
    }
    public abstract class WaterImageBuilder : ImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        protected string WaterImgPath { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected byte[] WaterImgBuffter { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="waterImgPath"></param>
        public void SetWaterImage(  string waterImgPath)
        {
            if (!File.Exists(waterImgPath)) throw new FileNotFoundException("File Not Found", waterImgPath);
            WaterImgPath = waterImgPath;
            WaterImgBuffter = File.ReadAllBytes(waterImgPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffter"></param>
        public void SetWaterImage(  byte[] buffter)
        {
            if (buffter == null) throw new ArgumentNullException("buffter");
            WaterImgBuffter = buffter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceImg"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected virtual Rectangle GetWaterRectangle(Image sourceImg, Image waterImg)
        {
            return new Rectangle(0, 0, sourceImg.Width, sourceImg.Height);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            if (Opetion == null) throw new ImageException("Opetion is null");

          
            var sourceImg = SourceImgBuffter.ToImage();
            var waterImg = WaterImgBuffter.ToImage();  
            var trageSize = Opetion == null ? null : Opetion.TragetSize;
            Image tmpimg = trageSize != null ? new Bitmap(trageSize.Value.Width, trageSize.Value.Height) : new Bitmap(sourceImg.Width, sourceImg.Height);
            Graphics gType = CreateGraphics(tmpimg, sourceImg);
            var attributes = GetOpacity(Opetion.Opacity);
            var waterRectangle = GetWaterRectangle(sourceImg, waterImg);
            var tmpwatrer = CreateFillImage(waterRectangle, waterImg);
            try
            {
                gType.DrawImage(tmpwatrer, waterRectangle, 0, 0, tmpwatrer.Width, tmpwatrer.Height, GraphicsUnit.Pixel, attributes);
                return tmpimg;
            }
            finally
            {
              
                tmpwatrer.Dispose();
                sourceImg.Dispose();
                waterImg.Dispose();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tmpimg"></param>
        /// <param name="sourceImg"></param>
        /// <returns></returns>
        protected Graphics CreateGraphics(Image tmpimg, Image sourceImg)
        {
            Graphics gType = Graphics.FromImage(tmpimg);
            var trageSize = Opetion == null ? null : Opetion.TragetSize;
            if (trageSize != null)
            {
                gType.DrawImage(sourceImg, new Rectangle(new Point(0, 0), trageSize.Value), 0, 0, sourceImg.Width,
                    sourceImg.Height, GraphicsUnit.Pixel);
            }
            else
            {
                gType.DrawImage(sourceImg, new Point(0, 0));
            }
            return gType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected abstract Image CreateFillImage(Rectangle rectangle, Image waterImg);
    }

}
