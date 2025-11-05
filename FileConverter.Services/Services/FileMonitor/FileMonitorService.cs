using FileConverter.Core.Models;
using FileConverter.Core.Services.FileLoaderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileConverter.Service.Services.FileMonitor
{
    public class FileMonitorService
    {
        private readonly IEnumerable<IFileLoaderService> _loaders;
        private readonly string _directory;
        private readonly int _intervalSeconds;
        private readonly HashSet<string> _processedFiles = new();

        public event Action<List<TradeData>> OnDataLoaded;

        public FileMonitorService(IEnumerable<IFileLoaderService> loaders,string directory,int intervalSeconds)
        {
            _loaders = loaders;
            _directory = directory;
            _intervalSeconds = intervalSeconds;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                var files = Directory.GetFiles(_directory);
                var newFiles = files.Where(f => !_processedFiles.Contains(f)).ToList();

                if (newFiles.Any())
                {
                    var tasks = newFiles.Select(ProcessFileAsync);
                    await Task.WhenAll(tasks);
                }
                await Task.Delay(_intervalSeconds * 1000);
            }
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
            catch(Exception ex)
            {
                Console.WriteLine($"Error processing {filePath}: {ex.Message}");
            }
        }
    }
}
