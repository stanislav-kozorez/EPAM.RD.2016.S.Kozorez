using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem.Entities
{
    [Serializable]
    public class StorageInfo
    {
        public List<User> Users { get; set;}
        public string LastId { get; set; }
        public int GeneratorCallCount { get; set; }
        public string GeneratorTypeName { get; set; }
    }
}
