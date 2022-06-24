using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Tests.Acceptance.Models.Labs;

namespace DMX.Core.Api.Tests.Acceptance.Brokers
{
    public partial class DmxCoreApiBroker
    {
        private const string LabsApiRelativeUrl = "api/labs";

        public async ValueTask<Lab> PostLabAsync(Lab lab)
        {
            return await this.apiFactoryClient.PostContentAsync<Lab>(
                relativeUrl: $"{LabsApiRelativeUrl}", 
                content: lab);
        }
    }
}
