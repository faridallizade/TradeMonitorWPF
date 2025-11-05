using FileConverter.Core.Enums;
using FileConverter.Core.Models;
using FileConverter.Core.Services.FileLoaderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileConverter.Service.Services.FileLoader
{
    public class XmlLoaderService : IFileLoaderService
    {
        public string FileType => FileTypes.xml.ToString();

        public async Task<IEnumerable<TradeData>> LoadAsync(string filePath)
        {
            var xml = await File.ReadAllTextAsync(filePath);
            var doc = XDocument.Parse(xml);
            return doc.Descendants("value")
                .Select(v => new TradeData
                {
                    Date = DateTime.Parse(v.Attribute("date")?.Value),
                    Open = decimal.Parse(v.Attribute("open")?.Value),
                    High = decimal.Parse(v.Attribute("High")?.Value),
                    Low = decimal.Parse(v.Attribute("Low")?.Value),
                    Close = decimal.Parse(v.Attribute("Close")?.Value),
                    Volume = long.Parse(v.Attribute("Volume")?.Value)
                });
        }
    }
}
