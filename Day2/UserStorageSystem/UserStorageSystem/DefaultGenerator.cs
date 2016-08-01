using System;
using System.Collections;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    [Serializable]
    public class DefaultGenerator : MarshalByRefObject, IIdGenerator
    {
        private int number = 1;

        public int CallNumber { get; private set; }

        public int Current
        {
            get
            {
                return this.number;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public void RestoreGeneratorState(string lastId, int callAttemptCount)
        {
            int id;
            if (!int.TryParse(lastId, out id))
            {
                throw new ArgumentException("Invalid last id");
            }

            this.number = id;
            this.CallNumber = callAttemptCount;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            this.number = this.GetNextSimpleNumber();
            this.CallNumber++;
            if (this.CallNumber == 10000)
            {
                this.CallNumber = 0;
                this.number = 1;
            }

            return true;
        }

        public void Reset()
        {
            this.number = 1;
            this.CallNumber = 0;
        }

        private int GetNextSimpleNumber()
        {
            while (!this.IsSimple(++this.number))
            {
            }

            return this.number;
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