using Petroineos.PowerReport.Interfaces;

namespace Petroineos.PowerReport
{
    public class Local23PeriodMapper : IPeriodMapper
    {
        public string MapPeriod(int period)
        {
            period = period + 22;
            if (period > 23)
                period -= 24;
            var str = period.ToString("D2") + ":00";
            return str;
        }
    }
}
