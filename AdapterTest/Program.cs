using AdapterPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ITarget targetX = new AdapterX();
            Client client = new Client(targetX);
            client.Request();
            Console.ReadKey();
            ITarget targetY = new AdapterY();
            client = new Client(targetY);
            client.Request();
            Console.ReadKey();
        }
    }
}
