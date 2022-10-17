// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Reflection.Metadata;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowService
    {
        private void ValidateLabWorkflowOnAdd(LabWorkflow labWorkflow)
        {
            ValidateLabWorkflowIsNotNull(labWorkflow);

            Validate(
                (Rule: IsInvalid(labWorkflow.Id), Parameter: nameof(LabWorkflow.Id)),
                (Rule: IsInvalid(labWorkflow.Name), Parameter: nameof(LabWorkflow.Name)),
                (Rule: IsInvalid(labWorkflow.Owner), Parameter: nameof(LabWorkflow.Owner)),
                (Rule: IsInvalid(labWorkflow.CreatedBy), Parameter: nameof(LabWorkflow.CreatedBy)),
                (Rule: IsInvalid(labWorkflow.UpdatedBy), Parameter: nameof(LabWorkflow.UpdatedBy)),
                (Rule: IsInvalid(labWorkflow.CreatedDate), Parameter: nameof(LabWorkflow.CreatedDate)),
                (Rule: IsInvalid(labWorkflow.UpdatedDate), Parameter: nameof(LabWorkflow.UpdatedDate)),
                (Rule: IsInvalid(labWorkflow.Status), Parameter: nameof(LabWorkflow.Status)),

                (Rule: IsSame(
                    labWorkflow.UpdatedDate,
                    labWorkflow.CreatedDate,
                    nameof(LabWorkflow.CreatedDate)),
                Parameter: nameof(LabWorkflow.UpdatedDate)),

                (Rule: IsNotRecent(labWorkflow.CreatedDate), Parameter: nameof(LabWorkflow.CreatedDate)));
        }

        private static void ValidateLabWorkflowIsNotNull(LabWorkflow labWorkflow)
        {
            if (labWorkflow is null)
            {
                throw new NullLabWorkflowException();
            }
        }

        private static void ValidateLabWorkflowExists(
            LabWorkflow maybeLabWorkflow,
            Guid labworkflowId)
        {
            if (maybeLabWorkflow is null)
            {
                throw new NotFoundLabWorkflowException(labworkflowId);
            }
        }

        private static void ValidateLabWorkflowId(Guid labWorkflowId) =>
            Validate((Rule: IsInvalid(labWorkflowId), Parameter: nameof(LabWorkflow.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(ulong userId) => new
        {
            Condition = userId == default,
            Message = "User is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(LabWorkflowStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static dynamic IsSame(
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

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLabWorkflowException = new InvalidLabWorkflowException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLabWorkflowException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidLabWorkflowException.ThrowIfContainsErrors();
        }
    }
}
