using System;

namespace BingoX.Draw
{
    /// <summary>
    /// 
    /// </summary>
    public struct Rational
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Rational GetRational(byte[] B)
        {
            Rational R = new Rational();

            R.Denominator = BitConverter.ToInt32(B, 0);
            R.Numerator = BitConverter.ToInt32(B, 4);
            return R;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Numerator;
        /// <summary>
        /// 
        /// </summary>
        public int Denominator;


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
