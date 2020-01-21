using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.ComponentModel
{
    public abstract class AssemblyScan
    {
        public AssemblyScan(Assembly assembly, Type constraintType)
        {
            Assembly = assembly;
            ConstraintType = constraintType;
        }

        public Assembly Assembly { get; protected set; }
        public Type ConstraintType { get; protected set; }
    }

    public class AssemblyScanInterface : AssemblyScan
    {
        public AssemblyScanInterface(Assembly assembly, Type constraintType, Type[] constraintGenericArguments) : this(assembly, constraintType)
        {

            ConstraintGenericArguments = constraintGenericArguments;
        }
        public AssemblyScanInterface(Assembly assembly, Type constraintType) : base(assembly, constraintType)
        {
            if (!constraintType.IsInterface) throw new LogicException("约束类型不为接口");

        }
        public Type[] ConstraintGenericArguments { get; protected set; }

        public Type[] Find()
        {
            if (ConstraintType.IsGenericType)
            {
                var typeGenericArguments = ConstraintType.GetGenericArguments();
                if (ConstraintGenericArguments != null)
                {
                    if (ConstraintGenericArguments.Length != typeGenericArguments.Length) throw new LogicException("约束的泛型参数不一致");
                    for (int i = 0; i < ConstraintGenericArguments.Length; i++)
                    {
                        var genericArgument = ConstraintGenericArguments[i];
                        var genericArgumentConstraintType = ConstraintGenericArguments[i];
                        if (!genericArgumentConstraintType.IsAssignableFrom(genericArgument)) throw new LogicException($"约束的泛型参数无一致性${genericArgument.FullName} ${genericArgumentConstraintType.FullName} ");
                    }
                }
                List<Type> list = new List<Type>();
                foreach (var interfaceType in Assembly.GetExportedTypes().Where(AssemblyHelper.IsInterfacePredicate))
                {

                    if (!interfaceType.IsGenericType) continue;
                    var genericArguments = interfaceType.GetGenericArguments();
                    if (genericArguments.Length != typeGenericArguments.Length) continue;
                    var genericType = ConstraintType.MakeGenericType(genericArguments);
                    if (genericType != interfaceType) continue;
                    list.Add(interfaceType);
                }
                return list.ToArray();
            }
            else
            {
                var list = Assembly.GetExportedTypes().Where(n => AssemblyHelper.IsInterfacePredicate(n) && AssemblyHelper.IsFromAndNotSelfPredicate(n, ConstraintType)).ToArray();


                return list;
            }
        }
    }


    public class AssemblyScanClass : AssemblyScan
    {
        public AssemblyScanClass(Assembly assembly, Type constraintType) : base(assembly, constraintType)
        {
            if (!constraintType.IsInterface) throw new LogicException("约束类型不为接口");
        }
        public bool BaseTypeIsGeneric { get; set; }
        public AssemblyScanResult[] Find()
        {

            List<AssemblyScanResult> list = new List<AssemblyScanResult>();
            if (ConstraintType.IsGenericType)
            {
                var typeGenericArguments = ConstraintType.GetGenericArguments();
                foreach (var item in Assembly.GetExportedTypes().Where(AssemblyHelper.IsClassPredicate))
                {
                    var interfaceType = item.GetInterfaces().FirstOrDefault(o => o.Name == ConstraintType.Name);
                    if (interfaceType == null) continue;
                    if (!interfaceType.IsGenericType) continue;
                    var genericArguments = interfaceType.GetGenericArguments();
                    if (genericArguments.Length != typeGenericArguments.Length) continue;
                    var genericType = ConstraintType.MakeGenericType(genericArguments);
                    if (genericType != interfaceType) continue;
                    IEnumerable<AssemblyScanResult> tmpTypes = null;
                    if (!BaseTypeIsGeneric)
                    {
                        tmpTypes = item.GetInterfaces().Where(o => AssemblyHelper.IsFromAndNotSelfPredicate(o, genericType) && o.IsGenericType == false).Select(n => new AssemblyScanResult { BaseType = n, ImplementedType = item });


                    }
                    else
                    {
                        tmpTypes = item.GetInterfaces().Where(o => AssemblyHelper.IsFromAndNotSelfPredicate(o, genericType)).Select(n => new AssemblyScanResult { BaseType = n, ImplementedType = item });


                    }
                    if (tmpTypes.Count() == 0)
                    {
                        list.Add(new AssemblyScanResult { BaseType = genericType, ImplementedType = item });
                    }
                    else
                    {
                        list.AddRange(tmpTypes);
                    }

                }
            }
            else
            {
                foreach (var item in Assembly.GetExportedTypes().Where(n => AssemblyHelper.IsClassPredicate(n) && AssemblyHelper.IsFromPredicate(n, ConstraintType)))
                {
                    var interfaceType = item.GetInterfaces().FirstOrDefault(o => AssemblyHelper.IsFromAndNotSelfPredicate(o, ConstraintType) && o.IsGenericType == BaseTypeIsGeneric);
                    if (interfaceType != null)
                    {

                        list.Add(new AssemblyScanResult { BaseType = interfaceType, ImplementedType = item });
                    }
                    else
                    {
                        list.Add(new AssemblyScanResult { BaseType = ConstraintType, ImplementedType = item });
                    }
                }
            }
            return list.ToArray();
        }
    }
    public class AssemblyScanResult
    {
        public Type BaseType { get; internal set; }

        public Type ImplementedType { get; internal set; }
    }
}
