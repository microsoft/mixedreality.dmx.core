// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowService
    {
        private static void ValidateLabWorkflowId(Guid labWorkflowId) =>
            Validate((Rule: IsInvalid(labWorkflowId), Parameter: nameof(LabWorkflow.Id)));

        private static void ValidateLabWorkflowExists(
            LabWorkflow maybeLabWorkflow, 
            Guid labworkflowId)
        {
            if (maybeLabWorkflow is null)
            {
                throw new NotFoundLabWorkflowException(labworkflowId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

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
