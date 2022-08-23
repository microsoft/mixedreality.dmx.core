// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;

namespace DMX.Core.Api.Services.Orchestrations.Labs
{
    public partial class LabOrchestrationService
    {
        private void ValidateLabOnAdd(Lab lab) =>
            ValidateLabIsNotNull(lab);

        private static void ValidateLabIsNotNull(Lab lab)
        {
            if (lab is null)
            {
                throw new NullLabOrchestrationException();
            }
        }

        private static void ValidateLabId(Guid labId) =>
            Validate((Rule: IsInvalid(labId), Parameter: nameof(Lab.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required",
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLabException = new InvalidLabOrchestrationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLabException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidLabException.ThrowIfContainsErrors();
        }
    }
}
