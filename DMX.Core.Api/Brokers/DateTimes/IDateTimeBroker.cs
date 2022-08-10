// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace DMX.Core.Api.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        public DateTimeOffset GetCurrentDateTime(); 
    }
}
