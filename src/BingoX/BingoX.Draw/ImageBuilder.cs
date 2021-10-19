using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ImageBuilder : IImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        public event ImageCompletedEventHandler ProcessCompleted;

        /// <summary>
        ///
        /// </summary>
        protected string SourceImgPath { get; private set; }

        /// <summary>
        ///
        /// </summary>
        protected byte[] SourceImgBuffter { get; private set; }

        private Image _source;

        public ImageExif Exif { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public Image Source
        {
            get
            {
                if (_source != null) return _source;
                if (SourceImgBuffter != null)
                {                   
                    _source = SourceImgBuffter.ToImage();
                    return _source;
                }
                if (File.Exists(SourceImgPath))
                {
                    _source = new Bitmap(SourceImgPath);

                    return _source;
                }
                return null;
            }
        }
        public void Rotate(RotateFlipType rotate)
        {
            if (_source == null) return;
            if (_source is Bitmap == false) return;
            _source.RotateFlip(rotate);
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual ImageOption CreateOption()
        {
            return new ImageOption();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        public void SetSourceImage(Image source)
        {
            if (source == null) throw new ArgumentNullException("source");
            this._source = source;
            Exif = ImageExif.GetExifInfo(_source);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceImgPath"></param>
        public void SetSourceImage(string sourceImgPath)
        {
            if (!File.Exists(sourceImgPath)) throw new FileNotFoundException("文件不存在", sourceImgPath);
            SourceImgPath = sourceImgPath;
            SetSourceImage(File.ReadAllBytes(sourceImgPath));
           
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffter"></param>
        public void SetSourceImage(byte[] buffter)
        {
            if (buffter == null) throw new ArgumentNullException("buffter");
            SourceImgBuffter = buffter;
            Exif = ImageExif.GetExifInfo(SourceImgBuffter);
        }



        /// <summary>
        ///
        /// </summary>
        public unsafe void UnsafeProcessBitmapAsync()
        {
            BackgroundWorker background = new BackgroundWorker();
            Image image = this._source;
            background.DoWork += (x, y) =>
            {
                image = UnsafeProcessBitmap();
            };
            background.RunWorkerCompleted += (x, y) =>
            {
                //y.Error
                OnProcessBitmapCompleted(y.Error == null ? new ImageEventArgs(image) : new ImageEventArgs(y.Error));
            };

            background.RunWorkerAsync();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual unsafe Image UnsafeProcessBitmap()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual ImageOption Opetion { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opetion"></param>
        public virtual void SetOpetion(ImageOption opetion)
        {
            if (opetion == null) throw new ArgumentNullException("opetion");
            Opetion = opetion;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opacity"></param>
        public virtual void SetOpacity(float opacity)
        {
            InitOption();
            Opetion.Opacity = opacity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="size"></param>
        public virtual void SetTrageSize(Size size)
        {
            InitOption();
            Opetion.TragetSize = size;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitOption()
        {
            if (Opetion == null) Opetion = new ImageOption();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract Image ProcessBitmap();

        /// <summary>
        ///
        /// </summary>
        public void ProcessBitmapAsync()
        {
            BackgroundWorker background = new BackgroundWorker();
            Image image = this._source;
            background.DoWork += (x, y) =>
            {
                image = ProcessBitmap();
            };
            background.RunWorkerCompleted += (x, y) =>
            {
                OnProcessBitmapCompleted(y.Error == null ? new ImageEventArgs(image) : new ImageEventArgs(y.Error));
            };
            background.RunWorkerAsync();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opacity"></param>
        /// <returns></returns>
        protected ImageAttributes GetOpacity(float opacity)
        {
            float[][] nArray =
            {
                new[] {1f, 0, 0, 0, 0},
                new[] {0, 1f, 0, 0, 0},
                new[] {0, 0, 1f, 0, 0},
                new[] {0, 0, 0,opacity , 0},
                new[] {0, 0, 0, 0, 1f}
            };
            ColorMatrix matrix = new ColorMatrix(nArray);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return attributes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="processable"></param>
        /// <param name="sourcePath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static TryResult ProcessBitmap(IImageBuilder processable, string sourcePath, string savePath)
        {
            if (processable == null) return false;
            if (!File.Exists(sourcePath)) return false;

            try
            {
                processable.SetSourceImage(sourcePath);
                var image = processable.ProcessBitmap();
                image.Save(savePath);
            }
            catch (Exception ex)
            {
                return ex;
            }

            return true;
        }

        /// <summary>
        /// 色彩值漏出處理
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        protected byte Truncate(int a)
        {
            if (a < 0)
                return 0;
            if (a > 255)
                return 255;
            return (byte)a;
        }

        /// <summary>
        /// 色彩值漏出處理
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        protected byte Truncate(float a)
        {
            if (a < 0)
                return 0;
            if (a > 255)
                return 255;
            return (byte)a;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnProcessBitmapCompleted(ImageEventArgs e)
        {
            var handler = ProcessCompleted;
            if (handler != null) handler(this, e);
        }
    }
}
