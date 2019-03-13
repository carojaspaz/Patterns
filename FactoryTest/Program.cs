using FactoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // An array of creators
            Creator[] creators = new Creator[2];

            creators[0] = new CreatorA();
            creators[1] = new CreatorB();

            // Iterate over creators and create products
            foreach (Creator creator in creators)
            {
                Product product = creator.FactoryMethod();
                Console.WriteLine("Created {0}",
                  product.GetType().Name);
            }

            // Wait for user
            Console.ReadKey();
        }
    }
}
