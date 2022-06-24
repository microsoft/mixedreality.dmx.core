// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Tests.Acceptance.Brokers;
using DMX.Core.Api.Tests.Acceptance.Models.Labs;
using Tynamix.ObjectFiller;
using WireMock.Server;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.Labs
{
    [Collection(nameof(ApiTestCollection))]
    public partial class LabApiTests
    {
        private readonly DmxCoreApiBroker dmxCoreApiBroker;

        public LabApiTests(DmxCoreApiBroker dmxCoreApiBroker)
        {
            this.dmxCoreApiBroker = dmxCoreApiBroker;
        }

        private static Lab CreateRandomLab() =>
            new Filler<Lab>().Create();
    }
}
