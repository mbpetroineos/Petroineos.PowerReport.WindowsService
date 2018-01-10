using System;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using Petroineos.PowerReport.Interfaces;
using Services;

namespace Petroineos.PowerReport
{
    public class FileWriter : IFileWriter
    {
        private readonly ILog _log = LogManager.GetLogger("FileWriter");
        public void WritePowerPositionCsvFile(string filename, PowerTrade aggregatePowerTrade, IPeriodMapper periodMapper)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Local Time,Volume");
                foreach (var powerPeriod in aggregatePowerTrade.Periods.OrderBy(p => p.Period))
                {
                    var localTimeString = periodMapper.MapPeriod(powerPeriod.Period);
                    sb.AppendLine(string.Format("{0},{1:F0}", localTimeString, powerPeriod.Volume));
                }

                File.WriteAllText(filename, sb.ToString());
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Unable to write file {0}. {1}", filename, ex.Message);
                throw;
            }
        }

        public void WriteEmptyFile(string filename)
        {
            File.WriteAllText(filename,"");
        }
    }
}
