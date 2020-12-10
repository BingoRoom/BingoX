using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace BingoX
{
    public static class Types
    {
        /// <summary>
        /// Object 类型
        /// </summary>
        public static readonly Type Object = typeof(Object);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type Type = typeof(Type);

        /// <summary>
        /// Stirng 类型
        /// </summary>
        public static readonly Type String = typeof(String);

        /// <summary>
        /// Char 类型
        /// </summary>
        public static readonly Type Char = typeof(Char);

        /// <summary>
        /// Boolean 类型
        /// </summary>
        public static readonly Type Boolean = typeof(Boolean);

        /// <summary>
        /// Byte 类型
        /// </summary>
        public static readonly Type Byte = typeof(Byte);

        /// <summary>
        /// SByte 类型
        /// </summary>
        public static readonly Type SByte = typeof(SByte);

        /// <summary>
        /// Int16 类型
        /// </summary>
        public static readonly Type Int16 = typeof(Int16);

        /// <summary>
        /// UInt16 类型
        /// </summary>
        public static readonly Type UInt16 = typeof(UInt16);

        /// <summary>
        /// Int32 类型
        /// </summary>
        public static readonly Type Int32 = typeof(Int32);

        /// <summary>
        /// UInt32 类型
        /// </summary>
        public static readonly Type UInt32 = typeof(UInt32);

        /// <summary>
        /// Int64 类型
        /// </summary>
        public static readonly Type Int64 = typeof(Int64);

        /// <summary>
        /// UInt64 类型
        /// </summary>
        public static readonly Type UInt64 = typeof(UInt64);

        /// <summary>
        /// Double 类型
        /// </summary>
        public static readonly Type Double = typeof(Double);

        /// <summary>
        /// Single 类型
        /// </summary>
        public static readonly Type Single = typeof(Single);

        /// <summary>
        /// Decimal 类型
        /// </summary>
        public static readonly Type Decimal = typeof(Decimal);

        /// <summary>
        /// Guid 类型
        /// </summary>
        public static readonly Type Guid = typeof(Guid);

        /// <summary>
        /// DateTime 类型
        /// </summary>
        public static readonly Type DateTime = typeof(DateTime);

        /// <summary>
        /// TimeSpan 类型
        /// </summary>
        public static readonly Type TimeSpan = typeof(TimeSpan);

        /// <summary>
        /// Nullable 类型
        /// </summary>
        public static readonly Type Nullable = typeof(Nullable<>);

        /// <summary>
        /// IEnumerable 类型
        /// </summary>
        public static readonly Type EnumerableofT = typeof(IEnumerable<>);

        /// <summary>
        /// IEnumerable 类型
        /// </summary>
        public static readonly Type Enumerable = typeof(System.Collections.IEnumerable);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type Dictionary = typeof(System.Collections.IDictionary);

        /// <summary>
        /// List 类型
        /// </summary>
        public static readonly Type ListOfT = typeof(List<>);
        /// <summary>
        /// Dictionary 类型
        /// </summary>
        public static readonly Type DictionaryOfT = typeof(Dictionary<,>);

        /// <summary>
        /// IListSource 类型
        /// </summary>
        public static readonly Type ListSource = typeof(System.ComponentModel.IListSource);

        /// <summary>
        /// StringDictionary 类型
        /// </summary>
        public static readonly Type StringDictionary = typeof(StringDictionary);

        /// <summary>
        /// NameValueCollection 类型
        /// </summary>
        public static readonly Type NameValueCollection = typeof(NameValueCollection);

    }
}
