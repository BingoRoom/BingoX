using BingoX.Utility;
using System.Data;

namespace BingoX.Helper 
{
    public static class DataHelper
    {
        public static T Cast<T>(this DataRow row, string name)
        {
            var obj = row[name];
            return ObjectUtility.Cast<T>(obj);
        }
    }
}
