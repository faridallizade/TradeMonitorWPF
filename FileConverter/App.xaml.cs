using FileConverter.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace FileConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MonitorSettings Settings { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Settings = config.GetSection("MonitorSettings")
                .Get<MonitorSettings>();
        }
    }

}
