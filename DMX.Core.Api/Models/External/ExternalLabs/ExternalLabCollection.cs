// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Devices.External.ExternalDevices;
using Newtonsoft.Json;

namespace DMX.Core.Api.Models.External.ExternalLabs
{
    public class ExternalLabCollection
    {
        [JsonProperty("Data")]
        public ExternalDevice[] Devices { get; set; }
    }
}
