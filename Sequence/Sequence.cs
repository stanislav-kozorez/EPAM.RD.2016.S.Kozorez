using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence
{
    public static class Sequence
    {
        public static int[] GetSequence(int limit)
        {
            if (limit < 0)
                throw new ArgumentException();

            var array = new int[limit];

            for (int i = 0; i < limit; i++)
                array[i] = i + 1;

            return array;
        }
    }
}
