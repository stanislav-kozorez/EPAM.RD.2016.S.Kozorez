﻿using System;
using System.Configuration;
using System.Diagnostics;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    /// <summary>
    /// Default logger that uses System.Diagnostics.TraceSource class.
    /// </summary>
    [Serializable]
    public class DefaultLogger : MarshalByRefObject, ILogger
    {
        private TraceSource traceSource;
        private int id;

        public DefaultLogger()
        {
            this.traceSource = new TraceSource("Logger");
        }

        public void Log(TraceEventType eventType, string message)
        {
            try
            {
                this.traceSource.TraceData(eventType, ++this.id, message);
                this.traceSource.Flush();
                this.traceSource.Close();
            }
            catch (ConfigurationException ex)
            {
                throw new ArgumentException("Unable to configure logger \"Logger\". Check App.config.", ex);
            }
        }
    }
}
