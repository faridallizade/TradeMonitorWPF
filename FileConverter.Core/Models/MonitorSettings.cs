using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Core.Models
{
    public class MonitorSettings
    {
        public string InputDirectory { get; set; }
        public int CheckFrequencySeconds { get; set; }
        public List<string> EnabledLoaders { get; set; }
    }
}
