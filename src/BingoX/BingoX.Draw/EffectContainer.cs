using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BingoX.Draw
{
    public sealed class EffectContainer
    {
        private readonly ImageBuilder[] builders;

        public EffectContainer(ImageBuilder[] builders)
        {
            this.builders = builders ?? throw new ArgumentNullException(nameof(builders));
        }
        public Image ProcessBitmap(Bitmap source)
        {
            var tmp = source.Clone() as Image;
            foreach (var item in builders)
            {
                item.SetSourceImage(tmp);
                tmp = item.ProcessBitmap();
            }
            return tmp;
        }
        public byte[] ProcessBitmap(byte[] source)
        {
            var tmp = source.ToImage();
            foreach (var item in builders)
            {
                item.SetSourceImage(tmp);
                tmp = item.ProcessBitmap();
            }
            return tmp.ToArray();
        }
    }
}
