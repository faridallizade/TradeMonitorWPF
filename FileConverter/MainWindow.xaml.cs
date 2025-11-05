using FileConverter;
using FileConverter.Core.Models;
using FileConverter.Service.Services.FileMonitor;
using System.Collections.ObjectModel;
using System.ComponentModel;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TradeData> TradeDataList { get; set; } = new();
    public string LastCheckedTime { get; set; }

    private FileMonitorService _fileMonitor;

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel(FileMonitorService fileMonitor)
    {
        _fileMonitor = fileMonitor;
        _fileMonitor.OnDataLoaded += AddTradeData;
    }

    private void AddTradeData(List<TradeData> data)
    {
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var item in data)
                TradeDataList.Add(item);
            LastCheckedTime = DateTime.Now.ToString("HH:mm:ss");
            OnPropertyChanged(nameof(LastCheckedTime));
        });
    }

   
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
