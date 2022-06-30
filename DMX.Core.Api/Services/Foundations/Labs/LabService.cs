// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.Labs;

namespace DMX.Core.Api.Services.Foundations.Labs
{
    public partial class LabService : ILabService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public LabService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Lab> AddLabAsync(Lab lab) =>
        TryCatch(async () =>
        {
            ValidateLabOnAdd(lab);

            return await this.storageBroker.InsertLabAsync(lab);
        });

        public IQueryable<Lab> RetrieveAllLabsWithDevices() =>
        TryCatch(() => this.storageBroker.SelectAllLabsWithDevices());
    }
}
