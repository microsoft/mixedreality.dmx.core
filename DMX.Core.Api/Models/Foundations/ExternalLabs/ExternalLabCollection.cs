// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace DMX.Core.Api.Models.Foundations.ExternalLabs
{
    public class ExternalLabCollection
    {
        [JsonProperty("Data")]
        public ExternalLab[] ExternalLabs { get; set; }
    }
}
