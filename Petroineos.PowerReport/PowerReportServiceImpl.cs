using System;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using Petroineos.PowerReport.Interfaces;
using Services;

namespace Petroineos.PowerReport
{
    public class PowerReportServiceImpl : IPowerReportServiceImpl
    {
        private readonly IPowerService _powerService;
        private readonly IPowerTradeAggregator _aggregator;
        private readonly IFilenameCreator _filenameCreator;
        private readonly IFileWriter _fileWriter;
        private readonly IPeriodMapper _periodMapper;
        private IRunSettings _runSettings;

        private Timer _timer;

        private readonly ILog _log = LogManager.GetLogger("PowerReportServiceImpl");

        public PowerReportServiceImpl(IPowerService powerService, IPowerTradeAggregator aggregator,
            IFilenameCreator filenameCreator, IFileWriter fileWriter, IPeriodMapper periodMapper) 
        {
            _powerService = powerService;
            _aggregator = aggregator;
            _filenameCreator = filenameCreator;
            _fileWriter = fileWriter;
            _periodMapper = periodMapper;
        }

        public void Start()
        {
            _log.InfoFormat("Starting PowerReportServiceImpl");
            _runSettings = new RunSettings();
            _runSettings.Refresh();
            _log.InfoFormat("RunSettings: {0}", _runSettings);
            var tsIntervalInMs = TimeSpan.FromMinutes(_runSettings.IntervalInMinutes).TotalMilliseconds;
            _timer = new Timer{Interval = tsIntervalInMs};
            _timer.Elapsed += FetchTradesAndWriteToFile;
            _log.InfoFormat("Initial Fetch and Write");
            FetchTradesAndWriteToFile(null, null);
            _timer.Start();
            _log.InfoFormat("Timer started");
        }

        private void FetchTradesAndWriteToFile(object sender, ElapsedEventArgs e)
        {
            var ppfw = new PowerPositionFetcherWriter();
            Task.Factory.StartNew(() =>
                {
                    ppfw.FetchTradesAndWriteToFile(_powerService, _runSettings, DateTime.Now, _aggregator, _filenameCreator,
                        _fileWriter, _periodMapper);
                }
            ).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    _log.ErrorFormat("Error in FetchTradesAndWriteToFile Task. {0}", t.Exception.Message);
                    return;
                }
            });
        }

        public void Stop()
        {
            _log.InfoFormat("Stopping PowerReportServiceImpl");
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
            _log.InfoFormat("PowerReportServiceImpl stopped");
        }
    }
}
