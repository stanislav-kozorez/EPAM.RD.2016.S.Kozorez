using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem.Entities;

namespace UserStorageSystem.Interfaces
{
    public interface IIdGenerator: IEnumerator<int>
    {
        void RestoreGeneratorState(string lastId);
    }
}
