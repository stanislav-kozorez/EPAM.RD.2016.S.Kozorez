using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IteratorExample
{
    public static class IteratorGetter
    {
        public static IteratorExample GetIterator()
        {
            return new IteratorExample();
        }
    }
}
