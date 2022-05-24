// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace DMX.Core.Api.Models.External.ExternalLabs
{
    public class ExternalLabsCollection
    {
        [JsonProperty("Data")]
        public ExternalLab[] Devices { get; set; }
    }
}
