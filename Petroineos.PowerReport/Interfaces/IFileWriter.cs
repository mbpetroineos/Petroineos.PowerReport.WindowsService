using Services;

namespace Petroineos.PowerReport.Interfaces
{
    public interface IFileWriter
    {
        void WritePowerPositionCsvFile(string filename, PowerTrade aggregatePowerTrade, IPeriodMapper periodMapper);
        void WriteEmptyFile(string filename);
    }
}
