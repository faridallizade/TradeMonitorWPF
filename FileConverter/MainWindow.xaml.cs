using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class TradeData
{
    public DateTime Date { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public long Volume { get; set; }
}

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TradeData> TradeDataList { get; set; } = new();

    private string _lastCheckedTime;
    public string LastCheckedTime
    {
        get => _lastCheckedTime;
        set
        {
            _lastCheckedTime = value;
            OnPropertyChanged();
        }
    }

    public void AddTradeData(List<TradeData> data)
    {
        if (data == null || data.Count == 0)
            return;

        foreach (var d in data)
            TradeDataList.Add(d);

        LastCheckedTime = DateTime.Now.ToString("HH:mm:ss");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
}
