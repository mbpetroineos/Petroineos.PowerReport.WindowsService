using System;
using System.Configuration;
using Petroineos.PowerReport.Interfaces;

namespace Petroineos.PowerReport
{
    public class RunSettings : IRunSettings
    {
        public string Filename { get; private set; }
        public string Path { get; private set; }
        public int IntervalInMinutes { get; private set; }

        public void Refresh()
        {
            Filename = ConfigurationManager.AppSettings["filename"];
            Path = ConfigurationManager.AppSettings["path"];
            IntervalInMinutes = int.Parse(ConfigurationManager.AppSettings["interval"]);
            IntervalInMinutes = Math.Max(1, IntervalInMinutes);
            IntervalInMinutes = Math.Min(60 * 24, IntervalInMinutes);
        }

        public override string ToString()
        {
            return string.Format("Filename: {0}, Path: {1}, IntervalInMinutes: {2}", Filename, Path, IntervalInMinutes);
        }
    }
}
