using BingoX.ComponentModel.Compress;

using ICSharpCode.SharpZipLib.Zip;

using ICSharpCode.SharpZipLib.Tar;
using System;
using System.Collections.Generic;
using System.IO;
namespace BingoX.Compress
{
    public static class ICSharpCompressFactory
    {
        public static ICompress Zip()
        {
            return new ZipICSharpCompress();
        }
       
        public static ICompress Tar()
        {
            return new TarICSharpCompress();
        }



    }
}
