using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadePattern
{
    public class Facade
    {
        private SubSystemA subSystemA;
        private SubSystemB subSystemB;
        private SubSystemC subSystemC;

        public Facade()
        {
            subSystemA = new SubSystemA();
            subSystemB = new SubSystemB();
            subSystemC = new SubSystemC();
        }

        public void MethodA()
        {
            Console.WriteLine("\nMethod A -------");
            subSystemA.Method();
            subSystemB.Method();
        }

        public void MethodB()
        {
            Console.WriteLine("\nMethod B -------");
            subSystemB.Method();
            subSystemC.Method();
        }

        public void MethodC()
        {
            Console.WriteLine("\nMethod C -------");
            subSystemC.Method();
            subSystemA.Method();
        }
    }
}
