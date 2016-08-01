using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem.Entities;

namespace UserStorageSystem.Interfaces
{
    public interface IIdGenerator : IEnumerator<int>
    {
        int CallNumber { get; }

        void RestoreGeneratorState(string lastId, int callAttemptCount);
    }
}
