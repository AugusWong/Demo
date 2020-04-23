using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 表达式树
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        //表达式树主要由下面四部分组成:
    //        //1.body 主体部分
    //        //2.parameters 参数部分
    //        //3.nodetype   节点类型
    //        //4.lambda   表达式类型 
    //        Expression<Func<int, int, int>> expr = (x, y) => x + y;

    //        ParameterExpression exp1 = Expression.Parameter(typeof(int), "a");
    //        ParameterExpression exp2 = Expression.Parameter(typeof(int), "b");

    //        BinaryExpression exp = Expression.Multiply(exp1, exp2);
    //        var lamexp = Expression.Lambda<Func<int, int, int>>(exp, new ParameterExpression[] { exp1, exp2 });


    //    }
    //}


    class Program1
    {
        static void Main(string[] args)
        {
            var p = new Person()
            {
                Name = "jxq",
                Age = 23
            };
            var shallowCopy = Operator<Person>.ShallowCopy(p);
            shallowCopy.Name = "feichexia";
            Console.WriteLine(shallowCopy.Name);
            Console.WriteLine(p.Name);

            Console.ReadKey();
        }


    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public static class Operator<T>
    {
        private static readonly Func<T, T> ShallowClone;
        public static T ShallowCopy(T sourobj)
        {
            return ShallowClone.Invoke(sourobj);
        }
        static Operator()
        {
            var origParam = Expression.Parameter(typeof(T), "orig");

            // for each read/write property on T, create a  new binding 
            // (for the object initializer) that copies the original's value into the new object 
            var setProps = from prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           where prop.CanRead && prop.CanWrite
                           select (MemberBinding)Expression.Bind(prop, Expression.Property(origParam, prop));

            var body = Expression.MemberInit( // object initializer 
                Expression.New(typeof(T)), // ctor 
                setProps // property assignments 
            );

            ShallowClone = Expression.Lambda<Func<T, T>>(body, origParam).Compile();
        }
    }
}
