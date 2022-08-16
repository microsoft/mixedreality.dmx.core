// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
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
                (Rule: IsInvalid(labCommand.Status), Parameter: nameof(LabCommand.Status)),
                (Rule: IsInvalid(labCommand.Type), Parameter: nameof(LabCommand.Type)),
                (Rule: IsInvalid(labCommand.CreatedDate), Parameter: nameof(LabCommand.CreatedDate)),
                (Rule: IsInvalid(labCommand.UpdatedDate), Parameter: nameof(LabCommand.UpdatedDate)),

                (Rule: IsNotSame(
                    labCommand.UpdatedDate,
                    labCommand.CreatedDate,
                    nameof(LabCommand.CreatedDate)),
                Parameter: nameof(LabCommand.UpdatedDate)),

                (Rule: IsNotRecent(labCommand.CreatedDate), Parameter: nameof(LabCommand.CreatedDate))
                );
        }

        private static void ValidateLabCommandId(Guid labCommandId) =>
            Validate((Rule: IsInvalid(labCommandId), Parameter: nameof(LabCommand.Id)));

        private static void ValidateLabCommandExists(LabCommand maybeLabCommand, Guid labCommandId)
        {
            if (maybeLabCommand is null)
            {
                throw new NotFoundLabCommandException(labCommandId);
            }
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
            Message = "Value is not recognized"
        };

        private static dynamic IsInvalid(CommandType type) => new
        {
            Condition = Enum.IsDefined(type) is false,
            Message = "Value is not recognized"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string nameOfSecondDate) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {nameOfSecondDate}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

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
