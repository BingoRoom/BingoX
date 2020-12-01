using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Helper
{
    public static class ListBindingHelper
    {
        public static PropertyDescriptorCollection GetListItemProperties(Type type)
        {
            return TypeDescriptor.GetProperties(GetListItemType(type));
        }
        public static Type GetListItemType(object list)
        {
            if (list == null)
            {
                return null;
            }

            Type itemType = null;

            // special case for IListSource
            if ((list is Type) && (typeof(IListSource).IsAssignableFrom(list as Type)))
            {
                list = CreateBindingList(list as Type);
            }

            list = GetList(list);
            Type listType = (list is Type) ? (list as Type) : list.GetType();
            object listInstance = (list is Type) ? null : list;

            if (typeof(Array).IsAssignableFrom(listType))
            {
                itemType = listType.GetElementType();
            }
            else
            {
                PropertyInfo indexer = GetTypedIndexer(listType);

                if (indexer != null)
                {
                    itemType = indexer.PropertyType;
                }
                else if (listInstance is IEnumerable)
                {
                    itemType = GetFirstType(listInstance as IEnumerable);
                }
                else
                {
                    itemType = listType;
                }
            }

            return itemType;
        }
        public static Type GetFirstType(this IEnumerable iEnumerable)
        {

            object instance = GetFirst(iEnumerable);
            return (instance != null) ? instance.GetType() : typeof(object);
        }
        public static object GetFirst(this IEnumerable enumerable)
        {
            object instance = null;

            if (enumerable is IList list)
            {


                instance = (list.Count > 0) ? list[0] : null;
            }
            else
            {

                try
                {
                    IEnumerator listEnumerator = enumerable.GetEnumerator();

                    listEnumerator.Reset();

                    if (listEnumerator.MoveNext())
                        instance = listEnumerator.Current;


                    listEnumerator.Reset();
                }
                catch (NotSupportedException)
                {

                    instance = null;
                }
            }

            return instance;
        }
        private static bool IsListBasedType(Type type)
        {
            // check for IList, ITypedList, IListSource
            if (typeof(IList).IsAssignableFrom(type) ||
                typeof(ITypedList).IsAssignableFrom(type) ||
                typeof(IListSource).IsAssignableFrom(type))
            {
                return true;
            }

            // check for IList<>:
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                if (typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                {
                    return true;
                }
            }

            // check for SomeObject<T> : IList<T> / SomeObject : IList<(SpecificListObjectType)>
            foreach (Type curInterface in type.GetInterfaces())
            {
                if (curInterface.IsGenericType)
                {
                    if (typeof(IList<>).IsAssignableFrom(curInterface.GetGenericTypeDefinition()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private static PropertyInfo GetTypedIndexer(Type type)
        {
            PropertyInfo indexer = null;

            if (!IsListBasedType(type))
            {
                return null;
            }

            System.Reflection.PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int idx = 0; idx < props.Length; idx++)
            {
                if (props[idx].GetIndexParameters().Length > 0 && props[idx].PropertyType != typeof(object))
                {
                    indexer = props[idx];
                    //Prefer the standard indexer, if there is one
                    if (indexer.Name == "Item")
                    {
                        break;
                    }
                }
            }

            return indexer;
        }
        public static object GetList(object list)
        {
            if (list is IListSource listSource)
            {
                return listSource.GetList();
            }
            else
            {
                return list;
            }
        }
        public static PropertyDescriptorCollection GetListItemProperties(object list)
        {
            PropertyDescriptorCollection pdc;

            if (list == null)
            {
                return new PropertyDescriptorCollection(null);
            }
            else if (list is Type)
            {
                pdc = GetListItemProperties(list as Type);
            }
            else
            {
                object target = GetList(list);

                if (target is ITypedList)
                {
                    pdc = (target as ITypedList).GetItemProperties(null);
                }
                else if (target is IEnumerable)
                {
                    pdc = GetListItemProperties(target as IEnumerable);
                }
                else
                {
                    pdc = TypeDescriptor.GetProperties(target);
                }
            }

            return pdc;
        }
        public static PropertyDescriptorCollection GetListItemProperties(this IEnumerable enumerable)
        {
            PropertyDescriptorCollection pdc = null;
            Type targetType = enumerable.GetType();

            if (typeof(Array).IsAssignableFrom(targetType))
            {
                pdc = TypeDescriptor.GetProperties(targetType.GetElementType());
            }
            else
            {
                ITypedList typedListEnumerable = enumerable as ITypedList;
                if (typedListEnumerable != null)
                {
                    pdc = typedListEnumerable.GetItemProperties(null);
                }
                else
                {
                    PropertyInfo indexer = GetTypedIndexer(targetType);

                    if (indexer != null && !typeof(ICustomTypeDescriptor).IsAssignableFrom(indexer.PropertyType))
                    {
                        Type type = indexer.PropertyType;
                        pdc = TypeDescriptor.GetProperties(type);



                    }
                }
            }

            // See if we were successful - if not, return the shape of the first
            // item in the list
            if (null == pdc)
            {
                object instance = GetFirst(enumerable);
                if (enumerable is string)
                {
                    pdc = TypeDescriptor.GetProperties(enumerable);
                }
                else if (instance == null)
                {
                    pdc = new PropertyDescriptorCollection(null);
                }
                else
                {
                    pdc = TypeDescriptor.GetProperties(instance);

                    if (!(enumerable is IList) && (pdc == null || pdc.Count == 0))
                    {
                        pdc = TypeDescriptor.GetProperties(enumerable);
                    }
                }

            }

            // Return results
            return pdc;
        }

        private static IList CreateBindingList(Type type)
        {
            Type genericType = typeof(BindingList<>);
            Type bindingType = genericType.MakeGenericType(new Type[] { type });


            return Activator.CreateInstance(bindingType) as IList;
        }
    }
}
