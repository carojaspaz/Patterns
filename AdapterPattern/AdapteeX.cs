using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPattern
{
    public class AdapteeX
    {
        internal void MethodToInvoke()
        {
            Console.WriteLine("Method X");
        }
    }
}
