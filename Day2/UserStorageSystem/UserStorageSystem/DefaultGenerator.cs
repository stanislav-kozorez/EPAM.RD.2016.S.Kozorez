using System;
using System.Collections;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    internal class DefaultGenerator: IIdGenerator
    {
        private int _callAttemptNumber;
        private int _number = 1;

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

        public void RestoreGeneratorState(string lastId)
        {
            int id;
            if(!int.TryParse(lastId, out id))
            {
                throw new ArgumentException("Invalid last id");
            }
            while (_number != id)
            {
                MoveNext();
            }
        }

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            _number = GetNextSimpleNumber();
            _callAttemptNumber++;
            if (_callAttemptNumber == 10000)
            {
                _callAttemptNumber = 0;
                _number = 1;
            }
            return true;
        }

        public void Reset()
        {
            _number = 1;
            _callAttemptNumber = 0;
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