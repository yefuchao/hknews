using Dapper;
using HKExNews.Background.Configuration;
using HKExNews.Background.Tasks.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace HKExNews.Background.Tasks
{
    public class CheckStockDataStateService : BackgroundTask
    {
        const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket_c.aspx?t=hk";
        private readonly ILogger<CheckStockDataStateService> _logger;
        private readonly BackgroundTaskSettings _settings;

        public CheckStockDataStateService(IOptions<BackgroundTaskSettings> settings, ILogger<CheckStockDataStateService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() => _logger.LogDebug($"#1 GracePeriodManagerService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"GracePeriodManagerService background task is doing background work.");

                CheckDataState();

                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.LogDebug($"GracePeriodManagerService background task is stopping.");

            await Task.CompletedTask;
        }

        private void CheckDataState()
        {
            _logger.LogDebug($"Checking confirmed grace period orders");

            var lastday = GetLastDayStored();

            if (lastday.Length > 0)
            {
                //send event 
            }
        }

        private string GetLastDayStored()
        {
            using (var conn = new SqlConnection(_settings.ConnectionString))
            {
                try
                {
                    conn.Open();
                    var conmand = "select TOP 1 Date from DateSaved order by Id desc";
                    var date = conn.QueryFirst<string>(conmand);
                    return date;
                }
                catch (Exception exception)
                {
                    _logger.LogCritical($"FATAL ERROR: Database connections could not be opened: {exception.Message}");
                    return "";
                }
            }
        }








    }
}
