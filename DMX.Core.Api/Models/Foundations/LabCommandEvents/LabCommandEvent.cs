// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Queues;
using DMX.Core.Api.Models.Foundations.LabCommands;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents
{
    public class LabCommandEvent
    {
        public LabCommand LabCommand { get; set; }
    }
}
