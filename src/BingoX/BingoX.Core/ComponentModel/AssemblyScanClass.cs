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

        public AssemblyScanInterface(Assembly assembly, Type constraintType) : base(assembly, constraintType)
        {
            if (!constraintType.IsInterface) throw new LogicException("约束类型不为接口");

        }

        public Type[] Find()
        {
            if (ConstraintType.IsGenericType)
            {

                List<Type> list = new List<Type>();
                foreach (var interfaceType in Assembly.GetExportedTypes().Where(n => AssemblyHelper.IsInterfacePredicate(n) && n.GetInterfaces().Any(o => o.Name == ConstraintType.Name)))
                {
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


        public AssemblyScanResult[] Find(Assembly interfaceAssembly)
        {

            List<AssemblyScanResult> list = new List<AssemblyScanResult>();


            if (interfaceAssembly != null)
            {
                AssemblyScanInterface scanInterface = new AssemblyScanInterface(interfaceAssembly, ConstraintType);
                var interfacesReslut = scanInterface.Find();
                foreach (var item in interfacesReslut)
                {
                    var itemImplementedType = Assembly.GetExportedTypes()
                        .Where(n => AssemblyHelper.IsClassPredicate(n) && AssemblyHelper.IsFromAndNotSelfPredicate(n, item))
                        .Select(n => new AssemblyScanResult { BaseType = item, ImplementedType = n }).ToArray();
                    list.AddRange(itemImplementedType);
                }
            }
            if (list.Count == 0)
            {
                foreach (var itemImplementedType in Assembly.GetExportedTypes().Where(n => AssemblyHelper.IsClassPredicate(n) && n.GetInterfaces().Any(o => o.Name == ConstraintType.Name)))
                {
                    var basetype = itemImplementedType.GetInterfaces().FirstOrDefault(o => o.Name == ConstraintType.Name);

                    list.Add(new AssemblyScanResult { BaseType = basetype, ImplementedType = itemImplementedType });
                }
            }
            return list.ToArray();
        }


        public Type[] Find()
        {

            List<Type> list = new List<Type>();

            //if (BaseTypeIsGenericType)
            //{
            foreach (var itemImplementedType in Assembly.GetExportedTypes().Where(n => AssemblyHelper.IsClassPredicate(n) && n.GetInterfaces().Any(o => o.Name == ConstraintType.Name)))
            {


                list.Add(itemImplementedType);
            }
            //}
            //else
            //{
            //    AssemblyScanInterface scanInterface = new AssemblyScanInterface(Assembly, ConstraintType);
            //    var interfacesReslut = scanInterface.Find();
            //    foreach (var item in interfacesReslut)
            //    {
            //        var itemImplementedType = Assembly.GetExportedTypes()
            //            .Where(n => AssemblyHelper.IsClassPredicate(n) && AssemblyHelper.IsFromAndNotSelfPredicate(n, item))
            //            .Select(n => new AssemblyScanResult { BaseType = item, ImplementedType = n }).ToArray();
            //        list.AddRange(itemImplementedType);
            //    }
            //}
            return list.ToArray();
        }
    }
    public class AssemblyScanResult
    {
        public Type BaseType { get; internal set; }

        public Type ImplementedType { get; internal set; }
    }
}
