﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Blobs;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.LabArtifacts;

namespace DMX.Core.Api.Services.Foundations.LabArtifacts
{
    public partial class LabArtifactService : ILabArtifactService
    {
        private readonly IBlobBroker artifactsBroker;
        private readonly ILoggingBroker loggingBroker;

        public LabArtifactService(
            IBlobBroker artifactsBroker,
            ILoggingBroker loggingBroker)
        {
            this.artifactsBroker = artifactsBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask AddLabArtifactAsync(string labArtifactName, Stream labArtifactContent) =>
        TryCatch(async () =>
        {
            ValidateLabArtifactPropertiesOnAdd(labArtifactName, labArtifactContent);

            var labArtifact = new LabArtifact
            {
                Name = labArtifactName,
                Content = labArtifactContent
            };

            await this.artifactsBroker.UploadLabArtifactAsync(labArtifact);
        });
    }
}
