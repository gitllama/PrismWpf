using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDryloc_For_Prism7._1.Models
{
    public class ModelBase
    {
        public string a = "base";

        public ModelBase()
        {
            Console.WriteLine(a);
        }
    }

    public class Model_A : ModelBase
    {
        public Model_A()
        {
            a = "a";
            Console.WriteLine(a);
        }
    }

}
