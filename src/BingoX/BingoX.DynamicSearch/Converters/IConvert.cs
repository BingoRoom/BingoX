using System;
using System.Data;

namespace BingoX.DynamicSearch
{
    public interface IConvert
    {
        object Conver(object row, object cellvalue, string fieldName, Type fieldType);
    }
    public interface IConverterDataTable : IConvert
    {

        object Conver(DataRow row, object cellvalue, string fieldName, Type fieldType);
    }
    public class DelegateConvert<T> : IConvert<T>
    {
        public DelegateConvert(Func<T, object, string, Type, object> convert)
        {

            this.convert = convert;
        }
        Func<T, object, string, Type, object> convert;
        public object Conver(T obj, object cellvalue, string fieldName, Type fieldType)
        {
            return convert(obj, cellvalue, fieldName, fieldType);
        }

        public object Conver(object row, object cellvalue, string fieldName, Type fieldType)
        {
            return Conver((T)row, cellvalue, fieldName, fieldType);
        }
        public static DelegateConvert<T> Create(Func<T, object, string, Type, object> convert)
        {
            return new DelegateConvert<T>(convert);
        }
    }
    public interface IConvert<T> : IConvert

    {

        object Conver(T obj, object cellvalue, string fieldName, Type fieldType);
    }

}
