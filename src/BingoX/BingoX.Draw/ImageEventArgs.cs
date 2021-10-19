using System;
using System.Drawing;

namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    public class ImageEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        public Exception Error { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public Image Image { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image"></param>
        protected internal ImageEventArgs(Image image)
        {
            Image = image;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="error"></param>
        protected internal ImageEventArgs(Exception error)
        {
            Error = error;
        }
    }
}
