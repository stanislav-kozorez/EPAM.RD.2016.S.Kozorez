using System;

namespace UserStorageSystem.Entities
{
    /// <summary>
    ///     Used by master to store connection information about slaves
    /// </summary>
    [Serializable]
    public class TcpInfo
    {
        public string IpAddress { get; set; }

        public int Port { get; set; }
    }
}
