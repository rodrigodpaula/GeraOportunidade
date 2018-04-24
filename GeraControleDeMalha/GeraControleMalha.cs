using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GeraControleDeMalha
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
    public partial class GeraControleMalha : ServiceBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        private IContainer componentes;
        private EventLog logdevento;
        public GeraControleMalha(string[] args)
        {
            InitializeComponent();
            eventLog1 = new EventLog();
            string eventSourceName = ConfigurationManager.AppSettings.Get("ARQ_LOG");
            string logName = "teste123";
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }
        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            OnStop();
        }
        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Processando");
            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 60000; // 60 seconds  
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            //timer.Start();
            //// Update the service state to Start Pending.  
            //ServiceStatus serviceStatus = new ServiceStatus();
            //serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            //serviceStatus.dwWaitHint = 100000;
            //SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            //// Update the service state to Running.  
            //serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            //SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            List<RotationIN> lstTST = new Input().csvRotation();
            //List<RotationIN> lstTST = new Input().ddsRotation();



        }
        private int eventId = 1;
        protected override void OnStop()
        {
            eventLog1.WriteEntry("Controle de malha foi paradao.");
        }
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Controle de malha foi continuado.");
        }
        protected override void OnPause()
        {
            eventLog1.WriteEntry("Controle de malha está pausado.");
        }
        protected override void OnShutdown()
        {
            eventLog1.WriteEntry("Controle de malha foi desligado.");
        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoramento do Controle de Malha", EventLogEntryType.Information, eventId++);
        }
    }
}
