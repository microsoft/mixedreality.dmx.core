// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;

namespace DMX.Core.Api.Services.Foundations.LabCommands
{
    public partial class LabCommandService : ILabCommandService
    {
        private static void ValidateLabCommandId(Guid labCommandId) =>
            Validate((Rule: IsInvalid(labCommandId), Parameter: nameof(LabCommand.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLabCommandException = new InvalidLabCommandException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLabCommandException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidLabCommandException.ThrowIfContainsErrors();
        }
    }
}
