using FileConverter.Core.Models;
using FileConverter.Core.Services.FileLoaderService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileConverter.Service.Services.FileMonitor
{
    public class FileMonitorService
    {
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IEnumerable<IFileLoaderService> _loaders;
        private readonly HashSet<string> _processedFiles = new();

        public event Action<List<TradeData>> OnDataLoaded;

        public string InputDirectory { get; private set; }
        public int IntervalSeconds { get; private set; }

        public FileMonitorService(IEnumerable<IFileLoaderService> loaders, string directory, int intervalSeconds)
        {
            _loaders = loaders;
            InputDirectory = directory;
            IntervalSeconds = intervalSeconds;
        }

        public async Task StartAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            while (!token.IsCancellationRequested)
            {
                var files = Directory.GetFiles(InputDirectory);
                var newFiles = files.Where(f => !_processedFiles.Contains(f)).ToList();

                if (newFiles.Any())
                {
                    var tasks = newFiles.Select(ProcessFileAsync);
                    await Task.WhenAll(tasks);
                }

                await Task.Delay(IntervalSeconds * 1000);
            }
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async Task ProcessFileAsync(string filePath)
        {
            try
            {
                var ext = Path.GetExtension(filePath).TrimStart('.').ToLower();
                var loader = _loaders.FirstOrDefault(l => l.FileType.Equals(ext, StringComparison.OrdinalIgnoreCase));

                if (loader == null) return;

                var data = await loader.LoadAsync(filePath);
                _processedFiles.Add(filePath);
                OnDataLoaded?.Invoke(data.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {filePath}: {ex.Message}");
            }
        }



        public void UpdateSettings(string newDirectory, int newInterval)
        {
            InputDirectory = newDirectory;
            IntervalSeconds = newInterval;

            Stop();
            StartAsync();
        }
    }
}
