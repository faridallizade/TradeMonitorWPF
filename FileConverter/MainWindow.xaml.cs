using FileConverter;
using FileConverter.Core.Models;
using FileConverter.Service.Services.FileMonitor;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TradeData> TradeDataList { get; set; } = new();
    public string LastCheckedTime { get; set; }

    private readonly FileMonitorService _fileMonitor;

    public string InputDirectory { get; set; }
    public int Interval { get; set; }
    public ICommand ApplySettingsCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel(FileMonitorService fileMonitor)
    {
        _fileMonitor = fileMonitor;
        _fileMonitor.OnDataLoaded += AddTradeData;

        InputDirectory = _fileMonitor.InputDirectory;
        Interval = _fileMonitor.IntervalSeconds;

        ApplySettingsCommand = new RelayCommand(ApplySettings);
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

    private void ApplySettings()
    {
        _fileMonitor.UpdateSettings(InputDirectory, Interval);
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
