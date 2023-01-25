// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using DMX.Core.Api.Models.Foundations.LabArtifacts;
using DMX.Core.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using WireMock.Server;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.LabArtifacts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class LabArtifactApiTests : IDisposable
    {
        private readonly DmxCoreApiBroker dmxCoreApiBroker;
        private readonly WireMockServer wireMockServer;

        public LabArtifactApiTests(DmxCoreApiBroker dmxCoreApiBroker)
        {
            this.dmxCoreApiBroker = dmxCoreApiBroker;
            this.wireMockServer = WireMockServer.Start(6122);
        }

        private static LabArtifact CreateRandomLabArtifact() =>
            CreateLabArtifactFiller().Create();

        private static Filler<LabArtifact> CreateLabArtifactFiller()
        {
            var filler = new Filler<LabArtifact>();
            var memoryStream = new MemoryStream();

            filler.Setup()
                .OnType<Stream>()
                    .Use(memoryStream);

            return filler;
        }

        public void Dispose()
        {
            this.wireMockServer.Stop();
        }
    }
}
