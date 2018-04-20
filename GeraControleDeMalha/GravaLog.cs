using System.Configuration;
using System.Diagnostics;

namespace GeraControleDeMalha
{
    class GravaLog 
    {
        public void GravaACT(string msg)
        {
            geraLOGevento = new EventLog();
            string eventSourceName = ConfigurationManager.AppSettings.Get("ARQ_LOG");
            string logName = msg;
            geraLOGevento = new EventLog();
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }
            geraLOGevento.Source = eventSourceName;
            geraLOGevento.Log = logName;
        }
        private EventLog geraLOGevento;
    }
}
