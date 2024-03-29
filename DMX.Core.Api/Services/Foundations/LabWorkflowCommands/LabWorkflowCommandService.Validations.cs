﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using CommandType = DMX.Core.Api.Models.Foundations.LabWorkflowCommands.CommandType;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandService
    {
        private void ValidateLabWorkflowCommandOnAdd(LabWorkflowCommand labWorkflowCommand)
        {
            ValidateLabWorkflowCommandIsNotNull(labWorkflowCommand);

            Validate(
                (Rule: IsInvalid(labWorkflowCommand.Id), Parameter: nameof(LabWorkflowCommand.Id)),
                (Rule: IsInvalid(labWorkflowCommand.WorkflowId), Parameter: nameof(LabWorkflowCommand.WorkflowId)),
                (Rule: IsInvalid(labWorkflowCommand.Arguments), Parameter: nameof(LabWorkflowCommand.Arguments)),
                (Rule: IsInvalid(labWorkflowCommand.CreatedBy), Parameter: nameof(LabWorkflowCommand.CreatedBy)),
                (Rule: IsInvalid(labWorkflowCommand.UpdatedBy), Parameter: nameof(LabWorkflowCommand.UpdatedBy)),
                (Rule: IsInvalid(labWorkflowCommand.CreatedDate), Parameter: nameof(LabWorkflowCommand.CreatedDate)),
                (Rule: IsInvalid(labWorkflowCommand.UpdatedDate), Parameter: nameof(LabWorkflowCommand.UpdatedDate)),
                (Rule: IsInvalid(labWorkflowCommand.Type), Parameter: nameof(LabWorkflowCommand.Type)),
                (Rule: IsInvalid(labWorkflowCommand.Status), Parameter: nameof(LabWorkflowCommand.Status)),

                (Rule: IsNotSame(
                    labWorkflowCommand.UpdatedDate,
                    labWorkflowCommand.CreatedDate,
                    nameof(LabWorkflowCommand.CreatedDate)),
                Parameter: nameof(LabWorkflowCommand.UpdatedDate)),

                (Rule: IsNotRecent(labWorkflowCommand.CreatedDate), Parameter: nameof(LabWorkflowCommand.CreatedDate)));
        }

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
                Parameter: nameof(LabWorkflowCommand.UpdatedDate)),

            (Rule: IsNotRecent(labWorkflowCommand.UpdatedDate), nameof(LabWorkflowCommand.UpdatedDate)));
        }

        private void ValidateLabWorkflowCommandAgainstStorageLabWorkflowCommand(
            LabWorkflowCommand labWorkflowCommand,
            LabWorkflowCommand storageLabWorkflowCommand)
        {
            ValidateLabWorkflowCommandExists(storageLabWorkflowCommand, labWorkflowCommand.Id);

            Validate(
                (Rule: IsNotSameAsStorage(
                    labWorkflowCommand.CreatedDate,
                    storageLabWorkflowCommand.CreatedDate,
                    nameof(LabWorkflowCommand.CreatedDate)),
                Parameter: nameof(LabWorkflowCommand.CreatedDate)));
        }

        private void ValidateLabWorkflowCommandExists(
            LabWorkflowCommand maybeLabWorkflowCommand,
            Guid labWorkflowCommandId)
        {
            if (maybeLabWorkflowCommand is null)
            {
                throw new NotFoundLabWorkflowCommandException(labWorkflowCommandId);
            }
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

        private static dynamic IsInvalid(ulong userId) => new
        {
            Condition = userId == default,
            Message = "User is required"
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

        private static dynamic IsNotSame(
           DateTimeOffset firstDate,
           DateTimeOffset secondDate,
           string nameOfSecondDate) => new
           {
               Condition = firstDate != secondDate,
               Message = $"Date is not the same as {nameOfSecondDate}"
           };

        private static dynamic IsBefore(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string nameofSecondDate) => new
            {
                Condition = firstDate < secondDate,
                Message = $"Date cannot be before {nameofSecondDate}"
            };

        private dynamic IsNotRecent(DateTimeOffset updatedDate) => new
        {
            Condition = IsDateNotRecent(updatedDate),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset updatedDate)
        {
            DateTimeOffset currentTime = this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentTime.Subtract(updatedDate);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static dynamic IsNotSameAsStorage(
            DateTimeOffset inputDate,
            DateTimeOffset storageDate,
            string nameOfStorageDate) => new
            {
                Condition = inputDate != storageDate,
                Message = $"Date is not the same as stored {nameOfStorageDate}"
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
