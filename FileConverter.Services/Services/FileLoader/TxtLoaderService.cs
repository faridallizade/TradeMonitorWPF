using FileConverter.Core.Enums;
using FileConverter.Core.Models;
using FileConverter.Core.Services.FileLoaderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Service.Services.FileLoader
{
    public class TxtLoaderService : IFileLoaderService
    {
        public string FileType => FileTypes.txt.ToString();

        public async Task<IEnumerable<TradeData>> LoadAsync(string filePath)
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            var list = new List<TradeData>();
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(";");
                if (parts.Length != 6) continue;

                list.Add(new TradeData
                {
                    Date = DateTime.Parse(parts[0]),
                    Open = decimal.Parse(parts[1]),
                    High = decimal.Parse(parts[2]),
                    Low = decimal.Parse(parts[3]),
                    Close = decimal.Parse(parts[4]),
                    Volume = long.Parse(parts[5])
                });
            }
            return list;
        }
    }
}
