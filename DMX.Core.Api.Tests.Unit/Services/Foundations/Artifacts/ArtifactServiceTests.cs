// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Artifacts;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.Artifacts;
using DMX.Core.Api.Services.Foundations.Artifacts;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Artifacts
{
    public partial class ArtifactServiceTests
    {
        private readonly Mock<IArtifactsBroker> artifactBroker;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IArtifactService artifactService;

        public ArtifactServiceTests()
        {
            this.artifactBroker = new Mock<IArtifactsBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.artifactService = new ArtifactService(
                artifactsBroker: this.artifactBroker.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Artifact CreateRandomArtifact() =>
            CreateArtifactFiller().Create();

        private static Filler<Artifact> CreateArtifactFiller()
        {
            var filler = new Filler<Artifact>();
            Stream stream = new MemoryStream();

            filler.Setup()
                .OnType<Stream>()
                    .Use(stream);

            return filler;
        }
    }
}
