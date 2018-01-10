using System;
using System.Collections.Generic;
using System.Linq;
using Services;

namespace Petroineos.PowerReport
{
    public class PowerTradesSample
    {
        public DateTime Date { get; private set; }
        public DateTime SampleTime { get; private set; }
        public PowerTrade[] Trades { get; private set; }

        public PowerTradesSample(IEnumerable<PowerTrade> powerTrades)
        {
            SampleTime = DateTime.Now;
            Trades = powerTrades.ToArray();
            Date = (Trades.Length > 0)?Date = Trades[0].Date:SampleTime;
        }

        public PowerTradesSample(IEnumerable<PowerTrade> powerTrades, DateTime sampleTime)
        {
            SampleTime = sampleTime;
            Trades = powerTrades.ToArray();
            Date = (Trades.Length > 0) ? Date = Trades[0].Date : SampleTime;
        }
    }
}
