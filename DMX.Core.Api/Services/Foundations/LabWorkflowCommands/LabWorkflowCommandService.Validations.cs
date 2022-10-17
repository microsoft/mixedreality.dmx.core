// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using System;
using System.Reflection.Metadata;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandService
    {
        private void ValidateLabWorkflowCommandOnModify(LabWorkflowCommand labWorkflowCommand)
        {
            ValidateLabWorkflowCommandIsNotNull(labWorkflowCommand);

            Validate(
                (Rule: IsInvalid(labWorkflowCommand.Id), Parameter: nameof(LabWorkflowCommand.Id)),
                (Rule: IsInvalid(labWorkflowCommand.LabId), Parameter: nameof(LabWorkflowCommand.LabId)),
                (Rule: IsInvalid(labWorkflowCommand.WorkflowId), Parameter: nameof(LabWorkflowCommand.WorkflowId)),
                (Rule: IsInvalid(labWorkflowCommand.Type), Parameter: nameof(LabWorkflowCommand.Type)),
                (Rule: IsInvalid(labWorkflowCommand.Status), Parameter: nameof(LabWorkflowCommand.Status)),
                (Rule: IsInvalid(labWorkflowCommand.Arguments), Parameter: nameof(LabWorkflowCommand.Arguments)),
                (Rule: IsInvalid(labWorkflowCommand.CreatedDate), Parameter: nameof(LabWorkflowCommand.CreatedDate)),
                (Rule: IsInvalid(labWorkflowCommand.UpdatedDate), Parameter: nameof(LabWorkflowCommand.UpdatedDate)),

                (Rule: IsSame(
                    labWorkflowCommand.UpdatedDate,
                    labWorkflowCommand.CreatedDate,
                    nameof(LabWorkflowCommand.CreatedDate)),
                Parameter: nameof(LabWorkflowCommand.UpdatedDate)),

            (Rule: IsBefore(
                    labWorkflowCommand.UpdatedDate,
                    labWorkflowCommand.CreatedDate,
                    nameof(LabWorkflowCommand.CreatedDate)),
                Parameter: nameof(LabWorkflowCommand.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required",
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required",
        };

        private static dynamic IsInvalid(DateTimeOffset dateTimeOffset) => new
        {
            Condition = dateTimeOffset == default,
            Message = "Date is required",
        };

        private static dynamic IsInvalid(CommandType type) => new
        {
            Condition = Enum.IsDefined(type) is false,
            Message = "Value is not recognized"
        };

        private static dynamic IsInvalid(CommandStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string nameofSecondDate) => new
        {
            Condition = firstDate != default && firstDate == secondDate,
            Message = $"Date is the same as {nameofSecondDate}"
        };

        private static dynamic IsBefore(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string nameofSecondDate) => new
            {
                Condition = firstDate < secondDate,
                Message = $"Date cannot be before {nameofSecondDate}"
            };

        public void ValidateLabWorkflowCommandIsNotNull(LabWorkflowCommand LabWorkflowCommand)
        {
            if (LabWorkflowCommand is null)
            {
                throw new NullLabWorkflowCommandException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLabWorkflowCommandException = new InvalidLabWorkflowCommandException(); 

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLabWorkflowCommandException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidLabWorkflowCommandException.ThrowIfContainsErrors();
        }
    }
}
