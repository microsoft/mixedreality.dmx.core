// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace DMX.Core.Api.Models.External.ExternalLabs
{
    public class ExternalLab
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ReservationId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsConnected { get; set; }
        public bool IsReserved { get; set; }
        public DateTimeOffset? StatusUpdatedTimeUtc { get; set; }
        public DateTimeOffset? LastActivityTimeUtc { get; set; }
        public DateTimeOffset? ReservationReleaseTimeUtc { get; set; }
        public IDictionary<string, string> Properties { get; set; }
        public IList<ExternalLabCommand> Commands { get; set; }
    }
}
