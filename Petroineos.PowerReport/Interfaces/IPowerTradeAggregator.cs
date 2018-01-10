using Services;

namespace Petroineos.PowerReport.Interfaces
{
    public interface IPowerTradeAggregator
    {
        PowerTrade Aggregate(PowerTradesSample powerTradeSample);
    }
}
