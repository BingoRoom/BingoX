using System;

namespace BingoX.Security
{
    public class RSAConvert
    {
        static RSAConvert()
        {
#if Standard
            XC = new XCRSAConvert();
#else
            BouncyCastle = new BouncyCastleRSAConvert();
#endif
        }

#if Standard
        public static IRSAConvet XC { get; set; }
#else
        public static IRSAConvet BouncyCastle{ get; set; }
#endif
    }
}
