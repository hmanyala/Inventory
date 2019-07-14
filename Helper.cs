using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Inventory
{
    public static class Helper
    {
        public static string JSONdata
        {
            get
            {
                var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                string path = configuration.GetSection("SourceFilePath").GetSection("FilePath").Value;
                return System.IO.File.ReadAllText(path);
            }
        }

        public static string FilePath
        {
            get
            {
                var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                return configuration.GetSection("SourceFilePath").GetSection("FilePath").Value;               
            }
        }


    }
}
