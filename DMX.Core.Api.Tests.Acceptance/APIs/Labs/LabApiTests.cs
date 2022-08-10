// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly WireMockServer wireMockServer;

        public LabApiTests(DmxCoreApiBroker dmxCoreApiBroker)
        {
            this.dmxCoreApiBroker = dmxCoreApiBroker;
            this.wireMockServer = WireMockServer.Start(6122);
        }

        private async ValueTask<Lab> PostRandomLabWithoutDevicesAsync()
        {
            Lab randomLab = CreateRandomLab();
            randomLab.Devices = null;
            await this.dmxCoreApiBroker.PostLabAsync(randomLab);

            return randomLab;
        }
        
        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomPowerLevel() =>
            new IntRange(min: 0, max: 101).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static bool GetRandomBoolean() => new Random().Next(2) == 1;

        private static List<Lab> CreateRandomLabsData() =>
            CreateRandomLabFiller().Create(count: GetRandomNumber()).ToList();

        private static Lab CreateRandomLab() =>
            CreateRandomLabFiller().Create();

        private static Filler<Lab> CreateRandomLabFiller() =>
             new Filler<Lab>();
    }
}