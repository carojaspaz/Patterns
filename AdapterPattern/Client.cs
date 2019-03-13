using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPattern
{
    public class Client
    {
        private ITarget Target;
        public Client(ITarget target)
        {
            Target = target;
        }

        public void Request()
        {
            Target.Method();
        }
    }
}
