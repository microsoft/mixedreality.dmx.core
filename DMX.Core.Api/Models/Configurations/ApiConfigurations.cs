// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

namespace DMX.Core.Api.Models.Configurations
{
    public class ApiConfigurations
    {
        public string Url { get; set; }
        public string GetAllDevicesAccessKey { get; internal set; }
        public string GetAvailableDevicesAccessKey { get; internal set; }
    }
}
