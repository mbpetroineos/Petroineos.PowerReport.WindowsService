using System;
using log4net;
using Petroineos.PowerReport.Interfaces;

namespace Petroineos.PowerReport
{
    public class FilenameCreator : IFilenameCreator
    {
        private ILog _log = LogManager.GetLogger("FilenameCreator");

        public string CreateFilename(IRunSettings runSettings, DateTime date, DateTime sampleTime)
        {
            var filename = runSettings.Filename.Replace("[yyyyMMdd]", date.ToString("yyyyMMdd"));
            filename = filename.Replace("[HHmm]", sampleTime.ToString("HHmm"));
            filename = runSettings.Path + filename;
            return filename;
        }

        public string CreateFilename(IRunSettings runSettings, PowerTradesSample powerTradesSample)
        {
            return CreateFilename(runSettings, powerTradesSample.Date, powerTradesSample.SampleTime);
        }
    }    
}
