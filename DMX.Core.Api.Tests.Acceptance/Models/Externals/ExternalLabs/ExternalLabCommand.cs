// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace DMX.Core.Api.Tests.Acceptance.Models.Externals.ExternalLabs
{
    public class ExternalLabCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
    }
}
