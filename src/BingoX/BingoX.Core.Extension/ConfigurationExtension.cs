using System;
using System.Linq;

namespace BingoX.Extensions
{
#if Standard
    public static class ConfigurationExtension
    {
        public static Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider GetConfigurationByFileName(this Microsoft.Extensions.Configuration.IConfiguration configuration, string configname)
        {
            var root = configuration as Microsoft.Extensions.Configuration.IConfigurationRoot;
            var chainedConfigurationProvider = root.Providers.OfType<Microsoft.Extensions.Configuration.ChainedConfigurationProvider>().First();
            string ENVName;
            chainedConfigurationProvider.TryGet("ENVIRONMENT", out ENVName);
            var filename = System.IO.Path.GetFileNameWithoutExtension(configname) + "." + ENVName + System.IO.Path.GetExtension(configname);
            var configFile = root.Providers.OfType<Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider>().FirstOrDefault(x => string.Equals(filename, x.Source.Path, StringComparison.InvariantCultureIgnoreCase));
            if (configFile != null) return configFile;

            configFile = root.Providers.OfType<Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider>().FirstOrDefault(x => string.Equals(configname, x.Source.Path, StringComparison.InvariantCultureIgnoreCase));
            return configFile;


        }
    }
#endif
}
