using System;
using log4net;
using Petroineos.PowerReport.Interfaces;
using Services;

namespace Petroineos.PowerReport
{
    public class PowerPositionFetcherWriter
    {
        private readonly ILog _log = LogManager.GetLogger("PowerPositionFetcherWriter");

        public void FetchTradesAndWriteToFile(IPowerService powerService, IRunSettings runSettings, DateTime runDateTime,
            IPowerTradeAggregator aggregator, IFilenameCreator filenameCreator, IFileWriter fileWriter,
            IPeriodMapper periodMapper)
        {
            try
            {
                var pts = new PowerTradesSample(powerService.GetTrades(runDateTime));

                var aggregatedTrades = aggregator.Aggregate(pts);
                var filename = filenameCreator.CreateFilename(runSettings, pts);
                fileWriter.WritePowerPositionCsvFile(filename, aggregatedTrades, periodMapper);
                _log.InfoFormat("File created from {0} trades ({1})", pts.Trades.Length, filename);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error in FetchTradesAndWriteToFile: {0}", ex.Message);                
                var filename = filenameCreator.CreateFilename(runSettings, runDateTime, DateTime.Now);
                _log.InfoFormat("Writing empty file ({0})", filename);
                fileWriter.WriteEmptyFile(filename);                
            }
        }
    }
}
