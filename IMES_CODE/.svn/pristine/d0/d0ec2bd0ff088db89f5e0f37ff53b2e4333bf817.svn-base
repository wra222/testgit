using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.ExpressionScript
{
    abstract class BaseClass
    {
        public virtual void Method1()
        {
            Console.WriteLine("Base - Method1");
        }
        public void Method2()
        {
            Console.WriteLine("Base - Method2");
        }
        abstract public void Method3();
    }

    class DerivedClass : BaseClass
    {
        public override void Method1()
        {
            Console.WriteLine("Derived - Method1");
        }

        new public void Method2()
        {
            Console.WriteLine("Derived - Method2");
        }
        public override void Method3()
        {
            Console.WriteLine("Derived - Method3");
        }
    }
}
