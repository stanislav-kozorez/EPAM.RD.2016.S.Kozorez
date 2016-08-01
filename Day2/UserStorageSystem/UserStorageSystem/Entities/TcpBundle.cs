using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem.Entities
{
    internal enum TcpCommand
    {
        Add,
        Delete
    }

    [Serializable]
    internal class TcpBundle
    {
        public TcpCommand Command { get; set; }

        public User User { get; set; }
    }
}
