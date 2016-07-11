using System;
using System.Collections;
using Sequence;

namespace IteratorExample
{
    public class IteratorExample : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Random r = new Random();

            var result = Sequence.Sequence.GetSequence(r.Next(3, 15));

            for (int i = 0; i < result.Length; i++)
                if (IsSimple(result[i]))
                    yield return result[i];
        }

        private bool IsSimple(int number)
        {
            bool result = true;
            for (int i = 2; i < number; i++)
            {
                if (number % i == 0)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}
