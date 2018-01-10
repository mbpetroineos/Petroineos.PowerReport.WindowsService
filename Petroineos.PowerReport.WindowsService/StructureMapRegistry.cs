using log4net;
using Petroineos.PowerReport.Interfaces;
using Services;
using StructureMap;
using StructureMap.Graph;

namespace Petroineos.PowerReport.WindowsService
{
    public class StructureMapRegistry : Registry
    {
        private static readonly ILog Log = LogManager.GetLogger("StructureMapRegistry");

        public StructureMapRegistry()
        {
            Log.Debug("Starting StructureMapRegistry constructor");

            Scan(reg =>
                {
                    reg.Assembly("Petroineos.PowerReport");
                    reg.WithDefaultConventions();
                }
            );

            For<ILog>().Use(context => LogManager.GetLogger(context.ParentType));

            For<IRunSettings>().Use(context => new RunSettings());
            For<IFilenameCreator>().Use(context => new FilenameCreator());
            For<IFileWriter>().Use(context => new FileWriter());
            For<IPeriodMapper>().Use(context => new Local23PeriodMapper());
            For<IPowerService>().Use(context => new PowerService());

            For<IPowerReportServiceImpl>().Use(context => new PowerReportServiceImpl(
                context.GetInstance<IPowerService>(),
                context.GetInstance<IPowerTradeAggregator>(),
                context.GetInstance<IFilenameCreator>(),
                context.GetInstance<IFileWriter>(),
                context.GetInstance<IPeriodMapper>()
                )
            );

            Log.Debug("Exiting StructureMapRegistry Constructor");
        }
    }
}
