using System.Diagnostics;

namespace UserStorageSystem.Interfaces
{
    /// <summary>
    ///     Implemented by classes that is used for logging
    /// </summary>
    public interface ILogger
    {
        void Log(TraceEventType eventType, string message);
    }
}
