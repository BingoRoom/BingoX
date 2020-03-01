using BingoX.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BingoX.Core.Test.RepositoryTest
{
    [Author("Dason")]
    [TestFixture]
    public class SpecificationTest
    {
        //[Test]
        //public void TestLambda()
        //{
        //    Lambda<MyClass>(n => n.Name);
        //}
        //[Test]
        //public void TestEnumerableContains()
        //{

        //    var method = typeof(Enumerable).GetMethods().FirstOrDefault(n => n.Name == "Contains");
        //    var containsDic = new Dictionary<Type, MethodInfo>
        //               {
        //               {typeof(string) ,     typeof(string).GetMethods().FirstOrDefault(n => n.Name == "Contains") },
        //                    {typeof(int) , method.MakeGenericMethod(typeof(int))  },
        //                    {typeof(long) , method.MakeGenericMethod(typeof(long))  },
        //                    {typeof(short) , method.MakeGenericMethod(typeof(short))  },
        //                    {typeof(uint) , method.MakeGenericMethod(typeof(uint))  },
        //                    {typeof(ulong) , method.MakeGenericMethod(typeof(ulong))  },
        //                    {typeof(ushort) , method.MakeGenericMethod(typeof(ushort))  },
        //                    {typeof(byte) , method.MakeGenericMethod(typeof(byte))  },
        //                    {typeof(char) , method.MakeGenericMethod(typeof(char))  },
        //                    {typeof(double) , method.MakeGenericMethod(typeof(double))  },
        //                    {typeof(decimal) , method.MakeGenericMethod(typeof(decimal))  },
        //                    {typeof(DateTime) , method.MakeGenericMethod(typeof(DateTime))  },


        //               };


        //}
        //[Test]
        //public void TestDynamicContains()
        //{

        //    MyClass[] arr = {
        //        new MyClass { Id=1,Age=10,Name="na"},
        //        new MyClass { Id=2,Age=19,Name="c"},
        //        new MyClass {Id=3,Age=20,Name="b" },
        //        new MyClass {Id=4,Age=10,Name="d" } };

        //    DynamicSpecification<MyClass> specifications = new DynamicSpecification<MyClass>();
        //    //   specifications.And("Id", new[] { 1, 2 });
        //    // Assert.IsTrue(specifications.IsSatisfiedBy(arr[0]));
        //    specifications = new DynamicSpecification<MyClass>();
        //    specifications.And("Name", "n");
        //    Assert.IsTrue(specifications.IsSatisfiedBy(arr[0]));
        //    DynamicSpecificationOption.StringCondition = StringCondition.Equals;
        //    specifications = new DynamicSpecification<MyClass>();
        //    specifications.And("Name", "n");
        //    Assert.IsFalse(specifications.IsSatisfiedBy(arr[0]));
        //}

        //[Test]
        //public void TestDynamicCamelCasePropertyException()
        //{

        //    var exception = Assert.Catch<SpecificationException>(() =>
        //         {
        //             DynamicSpecificationOption.CamelCaseProperty = false;
        //             var specifications = new DynamicSpecification<MyClass>();
        //             specifications.And("name", "n");
        //         });

        //    StringAssert.AreEqualIgnoringCase(exception.Message, "name字段不存在");

        //}

        //[Test]
        //public void TestDynamicCamelCaseProperty()
        //{


        //    var specifications = new DynamicSpecification<MyClass>();
        //    specifications.And("name", "n");
        //    specifications.And("Name", "n");


        //}
        //[Test]
        //public void TestOrder()
        //{
        //    MyClass[] arr = {
        //        new MyClass { Id=1,Age=10,Name="na"},
        //        new MyClass { Id=2,Age=19,Name="c"},
        //        new MyClass {Id=3,Age=20,Name="b" },
        //        new MyClass {Id=4,Age=10,Name="d" } };

        //    DynamicSpecification<MyClass> specifications = new DynamicSpecification<MyClass>();
        //    specifications.Orderby("Id", true);


        //}
        //class MyClass
        //{
        //    public string Name { get; set; }

        //    public int Id { get; set; }

        //    public int Age { get; set; }
        //}

        //private void Lambda<T>(Expression<Func<T, object>> orderExpression)
        //{
        //    var type = typeof(T);
        //    var param = Expression.Parameter(type, type.Name);
        //    var property = ((MemberExpression)orderExpression.Body).Member as PropertyInfo;
        //    Expression.Lambda(Expression.Property(param, property.Name), param);
        //}

    }
}
