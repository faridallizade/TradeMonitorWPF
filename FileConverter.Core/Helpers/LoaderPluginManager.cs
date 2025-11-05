using FileConverter.Core.Services.FileLoaderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Core.Helpers
{
    public class LoaderPluginManager
    {
        public static IEnumerable<IFileLoaderService> LoadPlugins(string folderPath)
        {
            var loaders = new List<IFileLoaderService>();

            if (!Directory.Exists(folderPath))
                return loaders;
            foreach (var file in Directory.GetFiles(folderPath, "*dll"))
            {
                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetTypes()
                    .Where(t => typeof(IFileLoaderService).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                foreach (var type in types)
                    loaders.Add((IFileLoaderService)Activator.CreateInstance(type));
            }
            return loaders;
        }
    }
}
