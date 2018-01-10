using System;
using StructureMap;
using Petroineos.PowerReport.Interfaces;
using Petroineos.PowerReport.WindowsService;

namespace Petroineos.PowerReport.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var container = new Container(new StructureMapRegistry());
            var impl = container.GetInstance<IPowerReportServiceImpl>();

            Console.WriteLine("Hosting in a console app");
            impl.Start();

            Console.WriteLine("Press [Return] to exit");
            Console.ReadLine();

            impl.Stop();

            Console.WriteLine("Goodbye");

        }
    }
}
