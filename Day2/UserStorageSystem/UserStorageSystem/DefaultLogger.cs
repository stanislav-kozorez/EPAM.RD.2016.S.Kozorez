using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class DefaultLogger : ILogger
    {
        private TraceSource _traceSource;
        private int _id;

        public DefaultLogger()
        {
            _traceSource = new TraceSource("Logger");
        }

        public void Log(TraceEventType eventType, string message)
        {
            try
            {
                _traceSource.TraceData(eventType, ++_id, message);
                _traceSource.Flush();
                _traceSource.Close();
            }
            catch (ConfigurationException ex)
            {
                throw new ArgumentException("Unable to configure logger \"Logger\". Check App.config.", ex);
            }
        }
    }
}
