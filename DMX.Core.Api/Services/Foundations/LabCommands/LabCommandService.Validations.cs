// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using Microsoft.IdentityModel.Tokens;
using CommandType = DMX.Core.Api.Models.Foundations.LabCommands.CommandType;

namespace DMX.Core.Api.Services.Foundations.LabCommands
{
    public partial class LabCommandService
    {
        private void ValidateLabCommandOnAdd(LabCommand labCommand)
        {
            ValidateLabCommandIsNotNull(labCommand);

            Validate(
                (Rule: IsInvalid(labCommand.Id), Parameter: nameof(LabCommand.Id)),
                (Rule: IsInvalid(labCommand.LabId), Parameter: nameof(LabCommand.LabId)),
                (Rule: IsInvalid(labCommand.Arguments), Parameter: nameof(LabCommand.Arguments)),
                (Rule: IsInvalid(labCommand.Notes), Parameter: nameof(LabCommand.Notes)),
                (Rule: IsInvalid(labCommand.Status), Parameter: nameof(LabCommand.Status)),
                (Rule: IsInvalid(labCommand.Type), Parameter: nameof(LabCommand.Type)));
        }

        private void ValidateLabCommandIsNotNull(LabCommand labCommand)
        {
            if (labCommand is null)
            {
                throw new NullLabCommandException();
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };
        
        private static dynamic IsInvalid(CommandStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognised"
        };
        
        private static dynamic IsInvalid(CommandType type) => new
        {
            Condition = Enum.IsDefined(type) is false,
            Message = "Value is not recognised"
        };

        private static void Validate(params (dynamic Rule,string Parameter)[] validations)
        {
            var invalidLabCommandException = new InvalidLabCommandException();

            foreach((dynamic rule, string parameter) in validations)
            {
                if(rule.Condition)
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
