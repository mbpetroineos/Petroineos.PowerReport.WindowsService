using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Petroineos.PowerReport.Interfaces;
using Services;

namespace Petroineos.PowerReport.Test
{
    [TestFixture]
    public class PowerReportTests
    {
        [Test]
        public void TestGetTrades()
        {
            TestSetupHelpers.SetTestMode();

            var ps = new PowerService();
            var date = new DateTime(2017,12,13);

            var trades = ps.GetTrades(date);

            Assert.AreEqual(2, trades.Count());

        }

        [Test]
        public void TestGetTradesError()
        {
            TestSetupHelpers.SetErrorMode();

            var ps = new PowerService();
            var date = new DateTime(2017, 12, 13);

            Assert.Throws<PowerServiceException>(()=> ps.GetTrades(date));

        }

        [Test]
        public void TestAggregatePowerTrades()
        {
            var powerTradesSample = TestSetupHelpers.GetPowerTradesSample();

            var ptAggregator = new PowerTradeAggregator();
            var aggregatedPowerTrade = ptAggregator.Aggregate(powerTradesSample);

            Assert.IsTrue(aggregatedPowerTrade.Periods.Length == 24);

            for (var i = 0; i < 24; i++)
            {
                var expected = (i + 1) * 6 * 10;
                Assert.AreEqual(expected, aggregatedPowerTrade.Periods[i].Volume, string.Format("Mismatch for period {0}", i));
            }
        }

        [Test]
        public void TestGetSettings()
        {
            var runSettings = new RunSettings();
            runSettings.Refresh();

            Assert.AreEqual("PowerPosition_[yyyyMMdd]_[HHmm].csv", runSettings.Filename);
            Assert.AreEqual(@"c:\temp\", runSettings.Path);
            Assert.AreEqual(5, runSettings.IntervalInMinutes);
        }

        [Test]
        public void TestCreateFilenameFromRunSettings()
        {
            var expected = @"c:\temp\PowerPosition_20171213_2350.csv";
            var runSettings = new RunSettings();
            runSettings.Refresh();

            var powerTradesSample = TestSetupHelpers.GetPowerTradesSample();

            var filenameCreator = new FilenameCreator();
            var filename = filenameCreator.CreateFilename(runSettings, powerTradesSample);

            Assert.AreEqual(expected, filename);
        }

        [Test]
        public void TestPeriodMapper()
        {
            var periodMapper = new Local23PeriodMapper();
            Assert.AreEqual("23:00", periodMapper.MapPeriod(1));
            Assert.AreEqual("00:00", periodMapper.MapPeriod(2));
            Assert.AreEqual("01:00", periodMapper.MapPeriod(3));
            Assert.AreEqual("02:00", periodMapper.MapPeriod(4));
            Assert.AreEqual("03:00", periodMapper.MapPeriod(5));
            Assert.AreEqual("04:00", periodMapper.MapPeriod(6));
            Assert.AreEqual("05:00", periodMapper.MapPeriod(7));
            Assert.AreEqual("06:00", periodMapper.MapPeriod(8));
            Assert.AreEqual("07:00", periodMapper.MapPeriod(9));
            Assert.AreEqual("08:00", periodMapper.MapPeriod(10));
            Assert.AreEqual("09:00", periodMapper.MapPeriod(11));
            Assert.AreEqual("10:00", periodMapper.MapPeriod(12));
            Assert.AreEqual("11:00", periodMapper.MapPeriod(13));
            Assert.AreEqual("12:00", periodMapper.MapPeriod(14));
            Assert.AreEqual("13:00", periodMapper.MapPeriod(15));
            Assert.AreEqual("14:00", periodMapper.MapPeriod(16));
            Assert.AreEqual("15:00", periodMapper.MapPeriod(17));
            Assert.AreEqual("16:00", periodMapper.MapPeriod(18));
            Assert.AreEqual("17:00", periodMapper.MapPeriod(19));
            Assert.AreEqual("18:00", periodMapper.MapPeriod(20));
            Assert.AreEqual("19:00", periodMapper.MapPeriod(21));
            Assert.AreEqual("20:00", periodMapper.MapPeriod(22));
            Assert.AreEqual("21:00", periodMapper.MapPeriod(23));
            Assert.AreEqual("22:00", periodMapper.MapPeriod(24));
        }

        [Test]
        public void TestSaveFile()
        {
            var runSettings = TestSetupHelpers.GetSaveFileRunSettings();

            var aggregatedTrades = TestSetupHelpers.GetPowerTrade1();

            var filenameCreator = new FilenameCreator();
            var filename = filenameCreator.CreateFilename(runSettings, aggregatedTrades.Date, new DateTime(2017, 12, 13, 23, 50, 55));

            var fileWriter = new FileWriter();

            var periodMapper = new Local23PeriodMapper();

            fileWriter.WritePowerPositionCsvFile(filename, aggregatedTrades, periodMapper);

            var expected = File.ReadAllLines(runSettings.Path + @"PowerPosition_20171213_2350_Expected.csv");
            var actual = File.ReadAllLines(filename);

            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [Test]
        public void TestEndToEndSuccessful()
        {
            TestSetupHelpers.SetTestMode();
            var ps = new PowerService();
            var ppfw = new PowerPositionFetcherWriter();
            var runSettings = TestSetupHelpers.GetSaveFileRunSettings();
            var date = new DateTime(2017, 12, 13, 23, 52, 00);

            var now = DateTime.Now;
            ppfw.FetchTradesAndWriteToFile(ps, runSettings, date, new PowerTradeAggregator(), new FilenameCreator(), 
                new FileWriter(), new Local23PeriodMapper());

            var expected = File.ReadAllLines(runSettings.Path + @"PowerPosition_20171213_" + now.ToString("HHmm") + ".csv");
            Assert.IsTrue(expected.Length >=25);
        }

        [Test]
        public void TestEndToEndError()
        {
            TestSetupHelpers.SetErrorMode();
            var ps = new PowerService();
            var ppfw = new PowerPositionFetcherWriter();
            var runSettings = TestSetupHelpers.GetSaveFileRunSettings();
            var date = new DateTime(2017, 12, 13, 23, 52, 00);

            var now = DateTime.Now;
            ppfw.FetchTradesAndWriteToFile(ps, runSettings, date, new PowerTradeAggregator(), new FilenameCreator(),
                new FileWriter(), new Local23PeriodMapper());

            var expected = File.ReadAllLines(runSettings.Path + @"PowerPosition_20171213_" + now.ToString("HHmm") + ".csv");
            Assert.IsTrue(expected.Length <= 0);
        }

    }

    public class TestRunSettings : IRunSettings
    {
        public string Filename { get;  set; }
        public string Path { get;  set; }
        public int IntervalInMinutes { get;  set; }

        public void Refresh()
        {

        }
    }
}
