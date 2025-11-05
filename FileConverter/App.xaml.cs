using FileConverter.Core.Models;
using FileConverter.Core.Services.FileLoaderService;
using FileConverter.Service.Services.FileLoader;
using FileConverter.Service.Services.FileMonitor;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using TradeMonitorApp;

namespace TradeMonitorWPF
{
    public partial class App : Application
    {
        public static MonitorSettings Settings { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var Settings = config.GetSection("MonitorSettings").Get<MonitorSettings>();

                if (Settings == null)
                    throw new Exception("MonitorSettings section is missing or invalid in appsettings.json.");

                var loaders = new List<IFileLoaderService>
                {
                    new CsvLoaderService(),
                    new XmlLoaderService(),
                    new TxtLoaderService()
                };

                var monitor = new FileMonitorService(
                    loaders,
                    Settings.InputDirectory,
                    Settings.CheckFrequencySeconds
                );

                var vm = new MainViewModel(monitor);

                var window = new MainWindow { DataContext = vm };
                window.Show();

                await monitor.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Config loading error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
