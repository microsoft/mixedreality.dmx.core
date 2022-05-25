using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Infrastructure.Provision.Models.Configurations;
using Microsoft.Extensions.Configuration;

namespace DMX.Core.Api.Infrastructure.Provision.Brokers.Configurations
{
    public class ConfigurationBroker : IConfigurationBroker
    {
        public CloudManagementConfiguration GetConfiguration()
        {
            IConfigurationRoot configrationRoot = new ConfigurationBuilder()
                .SetBasePath(basePath: Directory.GetCurrentDirectory())
                    .AddJsonFile(path: "appsettings.json", optional: false)
                        .Build();

            return configrationRoot.Get<CloudManagementConfiguration>();
        }
    }
}
