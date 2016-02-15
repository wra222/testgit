using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections;

namespace IMES.Infrastructure.ExpressionScript
{
    class Program
    {
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int Weight { get; set; }
            public DateTime FavouriteDay { get; set; }
            public string GetData(string d)
            {
                
                return  d;
            }
        }    
        static void Main()
        {                    
            const string exp = @"P.GetData(""test1234"").MatchGroup(""test(?<Name>[0-9]+)"")[""Name1""]==null && P.Weight > 50 || P.Age < 3";           
            //const string exp = @"""1234""+""ABC"" ";
            //const string exp =@"Info.Name==""AAA""";
            //const string exp = @"ProductInfo.123!=null";
            //const string exp = @"ProductInfo.GetData(""test"")==""test""";
            var p = Expression.Parameter(typeof(Person), "P");
            var p1 = Expression.Parameter(typeof(Person), "ProductInfo");
           //var p2 = Expression.Parameter(typeof(Func<Person, string>), "GetData");

          
            //var e = DynamicExpression.ParseLambda(new[] { p,p1,p2 },null, exp);
            var bob = new Person
            {
                Name = "Bob",
                Age = 30,
                Weight = 213,
                FavouriteDay = new DateTime(2000, 1, 1)
            };
                       
            //Func<Person, string> func11 = x=> x.GetData("test");
            //func11(bob);
          

            for (int i = 0; i < 10; ++i)
            {
                Delegate result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.Condition, "test", exp, p);

                var r = result.DynamicInvoke(bob);
                Console.WriteLine( i.ToString()+"  " +  r);
            }
            Console.ReadKey();
        }  

        static void test1(string[] args)
        {
       
            IList<Name> nameList = new List<Name>
            {
                new Name
                {
                    ID =1,
                    name="V",
                     remark="Vincent",
                },
                 new Name
                {
                    ID =2,
                    name="A",
                     remark="APPle",
                },
                new Name
                {
                    ID =3,
                    name="B",
                     remark="Book",
                },
                new Name
                {
                    ID =4,
                    name="B",
                     remark="Book",
                },
            };
            string where = "Regex.IsMatch(remark, \"B\")";
            string select="ID";
            var aaList = nameList.AsQueryable().Where(where).ToList();
            //var dd = nameList.Where(where).ToList();
            ParameterExpression x = Expression.Parameter(typeof(int), "x");
            ParameterExpression y = Expression.Parameter(typeof(int), "y");
            LambdaExpression e = DynamicExpression.ParseLambda(
                new ParameterExpression[] { x, y }, null, "(x + y) * 2");


        }

        static void test2()
        {
            //BaseClass bc = new BaseClass();
            DerivedClass dc = new DerivedClass();
            BaseClass bcdc = new DerivedClass();

            //bc.Method1();
            //bc.Method2();
            dc.Method1();
            dc.Method2();
            dc.Method3();
            bcdc.Method1();
            bcdc.Method2();
            bcdc.Method3();

            Type t = typeof(string);
            MethodInfo[] extendedMethods = t.GetExtensionMethods();
            MethodInfo extendedMethodInfo = t.GetExtensionMethod("IsMatch");
             
            System.Linq.Expressions.Expression<Func<int, int, bool>> largeSumTest =
    (num1, num2) => (num1 + num2) > 1000;

            // Create an InvocationExpression that represents applying
            // the arguments '539' and '281' to the lambda expression 'largeSumTest'.
            System.Linq.Expressions.InvocationExpression invocationExpression =
                System.Linq.Expressions.Expression.Invoke(
                    largeSumTest,
                    System.Linq.Expressions.Expression.Constant(539),
                    System.Linq.Expressions.Expression.Constant(281));

            Console.WriteLine(invocationExpression.ToString());
            Console.WriteLine(invocationExpression);
            // This code produces the following output:
            //
            // Invoke((num1, num2) => ((num1 + num2) > 1000),539,281)
            ////////////////////////////////////////////////////////////////
            Func<int, int, int> func = (int a, int b) => a + b;
            Expression<Func<int, int, int>> expr = (int a, int b) => a + b;
            int c = func(1, 2);

            DictionaryWithDefault<string, string> bb = "tes1234".MatchGroup("test(?<Name>[0-9]+)");
            string cc = bb["Name"];
            //////////////////////////////////////////////////////////////////////  
        }
    }

    public class Name
    {
        public int ID;
        public string name;
        public string remark;

    }
}
