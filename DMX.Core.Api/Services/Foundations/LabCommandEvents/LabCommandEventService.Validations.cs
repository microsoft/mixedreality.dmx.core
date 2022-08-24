// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;

namespace DMX.Core.Api.Services.Foundations.LabCommandEvents
{
    public partial class LabCommandEventService
    {
        private void ValidateLabCommandIsNotNull(LabCommand labCommand)
        {
            if (labCommand is null)
            {
                throw new NullLabCommandException();
            }
        }
    }
}
