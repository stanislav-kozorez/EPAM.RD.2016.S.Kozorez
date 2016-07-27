using System;
using System.Collections;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class DefaultGenerator: IIdGenerator
    {
        private int _number = 1;
        public int CallNumber { get; private set; }

        public int Current
        {
            get
            {
                return _number;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void RestoreGeneratorState(string lastId, int callAttemptCount)
        {
            int id;
            if(!int.TryParse(lastId, out id))
            {
                throw new ArgumentException("Invalid last id");
            }
            _number = id;
            CallNumber = callAttemptCount;
        }

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            _number = GetNextSimpleNumber();
            CallNumber++;
            if (CallNumber == 10000)
            {
                CallNumber = 0;
                _number = 1;
            }
            return true;
        }

        public void Reset()
        {
            _number = 1;
            CallNumber = 0;
        }

        private int GetNextSimpleNumber()
        {
            while (!IsSimple(++_number)) ;
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