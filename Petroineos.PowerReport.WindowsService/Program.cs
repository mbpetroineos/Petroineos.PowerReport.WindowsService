using System.ServiceProcess;
using Petroineos.PowerReport.Interfaces;
using StructureMap;

namespace Petroineos.PowerReport.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            var container = new Container(new StructureMapRegistry());
            var impl = container.GetInstance<IPowerReportServiceImpl>();

            var servicesToRun = new ServiceBase[]
            {
                new PowerReportService(impl),
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
