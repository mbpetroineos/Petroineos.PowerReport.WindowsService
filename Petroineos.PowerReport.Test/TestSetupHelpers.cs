using System;
using System.Collections.Generic;
using NUnit.Framework;
using Petroineos.PowerReport.Interfaces;
using Services;

namespace Petroineos.PowerReport.Test
{

    public static class TestSetupHelpers
    {
        public static void SetTestMode()
        {
            Environment.SetEnvironmentVariable("SERVICE_MODE", "Test");
        }

        public static void SetErrorMode()
        {
            Environment.SetEnvironmentVariable("SERVICE_MODE", "Error");
        }

        public static PowerTrade GetPowerTrade1()
        {
            var date = new DateTime(2017, 12, 13);
            var pt = PowerTrade.Create(date, 24);
            for (var i = 0; i < 24; i++)
            {
                pt.Periods[i] = new PowerPeriod
                {
                    Period = i + 1,
                    Volume = (i + 1) * 10
                };
            }
            return pt;
        }

        public static PowerTrade GetPowerTrade2()
        {
            var date = new DateTime(2017, 12, 13);
            var pt = PowerTrade.Create(date, 24);
            for (var i = 0; i < 24; i++)
            {
                pt.Periods[i] = new PowerPeriod
                {
                    Period = i + 1,
                    Volume = (i + 1) * 20
                };
            }
            return pt;
        }

        public static PowerTrade GetPowerTrade3()
        {
            var date = new DateTime(2017, 12, 13);
            var pt = PowerTrade.Create(date, 24);
            for (var i = 0; i < 24; i++)
            {
                pt.Periods[i] = new PowerPeriod
                {
                    Period = i + 1,
                    Volume = (i + 1) * 30
                };
            }
            return pt;
        }

        public static PowerTradesSample GetPowerTradesSample()
        {
            var pt1 = GetPowerTrade1();
            var pt2 = GetPowerTrade2();
            var pt3 = GetPowerTrade3();
            var powerTrades = new List<PowerTrade> { pt1, pt2, pt3 };
            var powerTradesSample = new PowerTradesSample(powerTrades, new DateTime(2017, 12, 13, 23, 50, 0));
            return powerTradesSample;
        }

        public static IRunSettings GetSaveFileRunSettings()
        {
            var configRunSettings = new RunSettings();
            configRunSettings.Refresh();

            var runSettings = new TestRunSettings
            {
                Filename = configRunSettings.Filename,
                Path = TestContext.CurrentContext.TestDirectory + @"\",
                IntervalInMinutes = configRunSettings.IntervalInMinutes
            };

            return runSettings;
        }
    }
}
