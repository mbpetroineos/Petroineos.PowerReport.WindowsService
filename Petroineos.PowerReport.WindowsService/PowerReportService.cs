using System.ServiceProcess;
using log4net;
using Petroineos.PowerReport.Interfaces;

namespace Petroineos.PowerReport.WindowsService
{
    partial class PowerReportService : ServiceBase
    {
        private readonly ILog _log = LogManager.GetLogger("PowerReportService");
        private readonly IPowerReportServiceImpl _impl;
        public PowerReportService(IPowerReportServiceImpl impl)
        {
            ServiceName = "Petroineos.PowerReportService";
            InitializeComponent();
            _impl = impl;
        }

        protected override void OnStart(string[] args)
        {
            _log.Info("Starting PowerReportService");
            _impl.Start();
            _log.Info("PowerReportService Started");
        }

        protected override void OnStop()
        {
            _log.Info("Stopping PowerReportService");
            _impl.Stop();
            _log.Info("PowerReportService Stopped");
        }
    }
}
