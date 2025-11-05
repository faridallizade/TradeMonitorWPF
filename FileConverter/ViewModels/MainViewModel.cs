using FileConverter.Core.Models;
using FileConverter.Service.Services.FileMonitor;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TradeMonitorWPF
{
    public class MainViewModel : ViewModelBase
    {
        private readonly FileMonitorService _fileMonitor;
        public ObservableCollection<TradeData> Trades { get; } = new();

        public string InputDirectory { get; set; }
        public int Interval { get; set; }
        public ICommand ApplySettingsCommand { get; }

        public MainViewModel(FileMonitorService fileMonitor)
        {
            _fileMonitor = fileMonitor;
            _fileMonitor.OnDataLoaded += AddTradeData;

            InputDirectory = _fileMonitor.InputDirectory;
            Interval = _fileMonitor.IntervalSeconds;

            ApplySettingsCommand = new RelayCommand(ApplySettings);
        }

        private void AddTradeData(IEnumerable<TradeData> newTrades)
        {
            foreach (var trade in newTrades)
                Trades.Add(trade);
        }

        private void ApplySettings()
        {
            _fileMonitor.UpdateSettings(InputDirectory, Interval);
        }
    }
}
