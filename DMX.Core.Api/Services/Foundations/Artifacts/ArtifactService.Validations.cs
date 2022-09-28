// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.Artifacts;
using DMX.Core.Api.Models.Foundations.Artifacts.Exceptions;
using Microsoft.Azure.ServiceBus;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection.Metadata;

namespace DMX.Core.Api.Services.Foundations.Artifacts
{
    public partial class ArtifactService
    {
        private void ValidateArtifactOnAdd(Artifact artifact)
        {
            ValidateArtifactIsNotNull(artifact);

            Validate(
                (Rule: IsInvalid(artifact.Name), Parameter: nameof(Artifact.Name)),
                (Rule: IsInvalid(artifact.Content), Parameter: nameof(Artifact.Content)));
        }

        private static void ValidateArtifactIsNotNull(Artifact artifact)
        {
            if (artifact is null)
            {
                throw new NullArtifactException();
            }
        }

        private static dynamic IsInvalid(string text) => new 
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required",
        };

        private static dynamic IsInvalid(Stream content) => new
        {
            Condition = content == null,
            Message = "Content is required",
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArtifactException = new InvalidArtifactException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArtifactException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArtifactException.ThrowIfContainsErrors();
        }
    }
}
