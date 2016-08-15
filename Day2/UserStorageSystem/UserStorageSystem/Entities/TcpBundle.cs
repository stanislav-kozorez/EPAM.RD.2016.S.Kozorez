using System;

namespace UserStorageSystem.Entities
{
    internal enum TcpCommand
    {
        Add,
        Delete
    }

    /// <summary>
    ///     Master uses TcpBundle to notify slaves about changes.
    /// </summary>
    [Serializable]
    internal class TcpBundle
    {
        public TcpCommand Command { get; set; }

        public User User { get; set; }
    }
}
