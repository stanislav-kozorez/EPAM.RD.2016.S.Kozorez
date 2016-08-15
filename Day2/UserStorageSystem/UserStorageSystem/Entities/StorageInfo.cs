using System;
using System.Collections.Generic;

namespace UserStorageSystem.Entities
{
    /// <summary>
    /// Used when saving data to file on disk
    /// </summary>
    [Serializable]
    public class StorageInfo
    {
        public List<User> Users { get; set; }

        public string LastId { get; set; }

        public int GeneratorCallCount { get; set; }

        public string GeneratorTypeName { get; set; }
    }
}
