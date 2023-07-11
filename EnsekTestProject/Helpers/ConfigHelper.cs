using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EnsekTestProject.Helpers
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot GetConfiguration ()
        {
            var configBuilder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .AddJsonFile("appsettings.json", true).Build();

            return configBuilder;
        
        }

    }
}

