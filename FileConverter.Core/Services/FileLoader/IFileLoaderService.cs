using FileConverter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Core.Services.FileLoaderService
{
    public interface IFileLoaderService
    {
        string FileType { get; }
        Task<IEnumerable<TradeData>> LoadAsync(string filePath);
    }
}
