using System;

namespace BingoX.Draw
{
    /// <summary>
    /// 
    /// </summary>
    public struct URational
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public static URational GetRational(byte[] B)
        {
            URational R = new URational();

            R.Denominator = BitConverter.ToUInt32(B, 0);
            R.Numerator = BitConverter.ToUInt32(B, 4);
            return R;
        }
        /// <summary>
        /// 
        /// </summary>
        public uint Numerator;
        /// <summary>
        /// 
        /// </summary>
        public uint Denominator;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("/");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Delimiter"></param>
        /// <returns></returns>
        public string ToString(string Delimiter)
        {
            return Numerator + "/" + Denominator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double ToDouble()
        {
            return (double)Numerator / Denominator;
        }
    }
}
