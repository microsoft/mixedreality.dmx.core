// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Infrastructure.Provision.Models.Configurations;

namespace DMX.Core.Api.Infrastructure.Provision.Brokers.Configurations
{
    public interface IConfigurationBroker
    {
        CloudManagementConfiguration GetConfiguration();
    }
}
