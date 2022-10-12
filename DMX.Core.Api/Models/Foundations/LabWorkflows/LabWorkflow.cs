// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows
{
    public class LabWorkflow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public List<LabWorkflowCommand> Commands { get; set; }
        public LabWorkflowStatus Status { get; set; }
        public string Notes { get; set; }
        public ulong CreatedBy { get; set; }
        public ulong UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public string Results { get; set; }
    }
}
