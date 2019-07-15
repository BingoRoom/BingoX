using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Utility
{
    /// <summary>
    /// 提供一个针对Type的操作工具
    /// </summary>
    public class TypeUtility
    {
        /// <summary>
        /// 反射一个指定类型
        /// </summary>
        /// <param name="stringtype">指定类型的类型名称</param>
        /// <returns>指定类型</returns>
        public static Type GetConcreteTypes(string stringtype)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(stringtype)).FirstOrDefault(type => type != null);
        }
        /// <summary>
        /// 反射一个指定类型
        /// </summary>
        /// <param name="classtype">指定类型的全命名名称</param>
        /// <returns>指定类型</returns>
        public static Type FindType(string classtype)
        {
            if (classtype == null) return null;
            var classarry = classtype.Split(',');
            var ass = Assembly.GetEntryAssembly();
            Type findclasstype = null;
            switch (classarry.Length)
            {
                case 1:
                    findclasstype = ass.GetType(classarry[0]);
                    break;
                case 2:
                    var assname = classarry[1];
                    if (string.Equals(System.IO.Path.GetFileNameWithoutExtension(ass.ManifestModule.ScopeName), assname, StringComparison.CurrentCultureIgnoreCase))
                    {
                        findclasstype = ass.GetType(classarry[0]);
                        break;
                    }
                    ass = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(n => string.Equals(System.IO.Path.GetFileNameWithoutExtension(n.ManifestModule.ScopeName), assname, StringComparison.CurrentCultureIgnoreCase));
                    if (ass != null) findclasstype = ass.GetType(classarry[0]);
                    break;
            }
            return findclasstype;
        }
    }
}
