﻿using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace BingoX.Draw.Effects
{
    /// <summary>
    /// 紅
    /// </summary>
    [Description("紅"), DisplayName("紅")]
    public class RedImage : ImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var pixelValue = bmp.GetPixel(column, row);
                    bmp.SetPixel(column, row, Color.FromArgb(pixelValue.A, pixelValue.R, 0, 0));
                }
            }
            return bmp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override unsafe Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ptr[0] = 0;//B
                    ptr[1] = 0;//G
                    ptr[2] = ptr[2];//R
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
    
}
