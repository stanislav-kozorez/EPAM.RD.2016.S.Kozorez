using System.Diagnostics;

namespace UserStorageSystem.Interfaces
{
    public interface ILogger
    {
        void Log(TraceEventType eventType, string message);
    }
}
