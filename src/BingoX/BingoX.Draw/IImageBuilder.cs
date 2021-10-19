using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ImageCompletedEventHandler(object sender, ImageEventArgs e);

    /// <summary>
    /// 图像处理功能
    /// </summary>
    public interface IImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        event ImageCompletedEventHandler ProcessCompleted;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceImgPath"></param>
        void SetSourceImage(string sourceImgPath);

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        void SetSourceImage(Image source);

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffter"></param>
        void SetSourceImage(byte[] buffter);

        /// <summary>
        ///
        /// </summary>
        /// <param name="opetion"></param>
        void SetOpetion(ImageOption opetion);

        /// <summary>
        /// .net自带处理方法
        /// </summary>
        Image ProcessBitmap();

        /// <summary>
        ///
        /// </summary>
        void ProcessBitmapAsync();

        /// <summary>
        ///
        /// </summary>
        unsafe void UnsafeProcessBitmapAsync();

        /// <summary>
        /// 不安全代码处理方法
        /// </summary>
        unsafe Image UnsafeProcessBitmap();

        /// <summary>
        ///
        /// </summary>
        /// <param name="opacity"></param>
        void SetOpacity(float opacity);

        /// <summary>
        ///
        /// </summary>
        /// <param name="size"></param>
        void SetTrageSize(Size size);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ImageOption CreateOption();
    }
}
