using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BingoX.Helper
{
    public static class AssemblyHelper
    {
        public static string ReadTextFile(this Assembly assembly, string name)
        {
            return ReadTextFile(assembly, name, Encoding.UTF8);
        }

        public static Type[] GetImplementedClass(this Assembly assembly, Type interfaceType, params Type[] genericType)
        {
            var types = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract);
            if (interfaceType.IsGenericType)
            {
                var maketype = interfaceType.MakeGenericType(genericType);
                return types.Where(o => maketype.IsAssignableFrom(o)).ToArray();
            }
            else
            {
                return types.Where(o => interfaceType.IsAssignableFrom(o)).ToArray();
            }


        }
        public static Type[] GetImplementedClass<TInterfaceType>(this Assembly assembly)
        {
            return GetImplementedClass(assembly, typeof(TInterfaceType));

        }

        public static string ReadTextFile(this Assembly assembly, string name, Encoding encoding)
        {
            var fs = assembly.GetManifestResourceStream(name);
            if (fs == null) throw new Exception("文件不存在");
            using (fs)
            {
                using (var reader = new System.IO.StreamReader(fs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
