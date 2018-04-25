using Dapper;
using EventBus.Abstractions;
using HKExNews.Background.Configuration;
using HKExNews.Background.IntegrationEvents;
using HKExNews.Background.Tasks.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace HKExNews.Background.Tasks
{
    public class CheckStockDataStateService : BackgroundTask
    {
        private readonly ILogger<CheckStockDataStateService> _logger;
        private readonly BackgroundTaskSettings _settings;
        private readonly IEventBus _eventBus;

        public CheckStockDataStateService(IOptions<BackgroundTaskSettings> settings, ILogger<CheckStockDataStateService> logger, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
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

            var yesterday = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");

            if (lastday != yesterday && lastday != "")
            {
                var date = DateTime.ParseExact(lastday, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                NoticeLastDayEvent notice = new NoticeLastDayEvent(date);

                _eventBus.Publish(notice);
            }
        }

        private string GetLastDayStored()
        {
            using (var conn = new MySqlConnection(_settings.ConnectionString))
            {
                try
                {
                    conn.Open();
                    var conmand = "select Date from DateSaved order by Id desc LIMIT 1";
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
