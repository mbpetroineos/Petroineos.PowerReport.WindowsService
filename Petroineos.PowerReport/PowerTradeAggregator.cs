using System;
using log4net;
using Petroineos.PowerReport.Interfaces;
using Services;

namespace Petroineos.PowerReport
{
    public class PowerTradeAggregator : IPowerTradeAggregator
    {
        private readonly ILog _log = LogManager.GetLogger("PowerTradeAggregator");
        private const int Periods = 24;

        public PowerTrade Aggregate(PowerTradesSample powerTradeSample)
        {
            try
            {
                // Beyond the scope for this but would be inclined to add some validation about the trades 
                // around dates and number of periods
                if (null == powerTradeSample.Trades || powerTradeSample.Trades.Length ==0)
                {
                    _log.Error("No trades to aggregate");
                    throw new Exception("No trades to aggregate");
                }

                var aggregatePeriods = new double[Periods];
                foreach (var powerTrade in powerTradeSample.Trades)
                {
                    foreach (var powerTradePeriod in powerTrade.Periods)
                    {
                        aggregatePeriods[powerTradePeriod.Period-1] += powerTradePeriod.Volume;
                    }
                }

                var agg = PowerTrade.Create(powerTradeSample.Date, Periods);
                for (var i = 0; i < Periods; i++)
                {
                    agg.Periods[i] = new PowerPeriod {Period = i+1, Volume = aggregatePeriods[i]};
                }

                return agg;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error in Aggregate. {0}",ex.Message);
                throw;
            }
        }

    }
}
