// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Blobs;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.LabArtifacts;
using DMX.Core.Api.Services.Foundations.LabArtifacts;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabArtifacts
{
    public partial class LabArtifactServiceTests
    {
        private readonly Mock<IBlobBroker> artifactBroker;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ILabArtifactService labArtifactService;

        public LabArtifactServiceTests()
        {
            this.artifactBroker = new Mock<IBlobBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labArtifactService = new LabArtifactService(
                artifactsBroker: this.artifactBroker.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static LabArtifact CreateRandomArtifact() =>
            CreateArtifactFiller().Create();

        private static Filler<LabArtifact> CreateArtifactFiller()
        {
            var filler = new Filler<LabArtifact>();
            Stream stream = new MemoryStream();

            filler.Setup()
                .OnType<Stream>()
                    .Use(stream);

            return filler;
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
