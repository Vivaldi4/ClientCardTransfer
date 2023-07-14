using ClientCardTransfer.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ClientCardTransfer.FileLoader;

namespace ClientCardTransfer.Service
{
    public class WorkService : BackgroundService
    {
        private readonly ILogger<WorkService> _logger;

        private readonly Setting _setting;
        private readonly TxtToSqlLoader _xtToSqlLoader;
        public WorkService(ILogger<WorkService> logger, Setting setting, TxtToSqlLoader txtToSqlLoader)
        {
            _logger = logger;
            _setting = setting;
            _xtToSqlLoader= txtToSqlLoader;
        }
        private static string ExtractValueAfterSubstring(string fileName, string substring)
        {
            int index = fileName.IndexOf(substring) + substring.Length;
            return fileName.Substring(index);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                // _logger.LogInformation($"Current folder: {_setting.Directory}", DateTimeOffset.Now);

                DirectoryInfo directory = new DirectoryInfo(Path.Combine(_setting.Directory));// Создаем объект DirectoryInfo для заданного пути

                //FileInfo[] files = directory.GetFiles();// Получаем массив файлов в указанной директории

                //string pattern = @"_?(\w+)$";
                //Regex regex = new Regex(pattern, RegexOptions.Compiled); Глупая машина посоветовала

                IEnumerable<string> clientFiles = Directory.EnumerateFiles(_setting.Directory, $"*{_setting.ClientName}*");
                IEnumerable<string> cardFiles = Directory.EnumerateFiles(_setting.Directory, $"*{_setting.CardName}*");
                foreach (var clientFile in clientFiles)
                {
                    string clientFileName = Path.GetFileName(clientFile);
                    string key1 = ExtractValueAfterSubstring(clientFileName, _setting.ClientName);

                    foreach (var cardFile in cardFiles)
                    {
                        string cardFileName = Path.GetFileName(cardFile);
                        string key2 = ExtractValueAfterSubstring(cardFileName, _setting.CardName);
                        if (string.Equals(key1, key2, StringComparison.OrdinalIgnoreCase))
                        {
                            _logger.LogInformation($"Пара найдена!!!Документ клиентов:{clientFile} Документ Карт клиентов{cardFile}");
                            
                            //TxtToSqlLoader txtToSql = new TxtToSqlLoader("DataBaseAddres");
                            //txtToSql.LoadFilesToSql(clientFile, cardFile);
                            _xtToSqlLoader.LoadFilesToSql(clientFile, cardFile);

                        }
                    }

                }







                // _logger.LogInformation($"Пара найдена!!!Документ клиентов:{files[fileClient].FullName} Документ Карт клиентов{files[fileCard].FullName}"); 

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}


