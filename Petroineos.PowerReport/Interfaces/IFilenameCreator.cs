using System;

namespace Petroineos.PowerReport.Interfaces
{
    public interface IFilenameCreator
    {
        string CreateFilename(IRunSettings runSettings, DateTime date, DateTime sampleTime);
        string CreateFilename(IRunSettings runSettings, PowerTradesSample powerTradesSample);
    }
}
