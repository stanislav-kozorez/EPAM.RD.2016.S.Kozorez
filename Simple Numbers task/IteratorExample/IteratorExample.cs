using System;
using System.Collections;

namespace IteratorExample
{
    public class IteratorExample : IEnumerable
    {
        private int _callAttemptNumber;
        private int _number = 1;       

        public IEnumerator GetEnumerator()
        {
            while(true)
            {
                yield return GetNextSimpleNumber();
                _callAttemptNumber++;
                if(_callAttemptNumber == 10000)
                {
                    _callAttemptNumber = 1;
                    _number = 1;
                }
            }
        }

        private int GetNextSimpleNumber()
        {
            while (!IsSimple(++_number));
            return _number;
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
