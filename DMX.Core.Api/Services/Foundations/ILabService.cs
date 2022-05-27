// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;

namespace DMX.Core.Api.Services.Foundations
{
    public interface ILabService
    {
        ValueTask<List<Lab>> RetrieveAllLabsAsync();
    }
}
