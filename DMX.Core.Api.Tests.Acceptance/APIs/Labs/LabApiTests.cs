// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using DMX.Core.Api.Tests.Acceptance.Brokers;
using DMX.Core.Api.Tests.Acceptance.Models.Labs;
using Tynamix.ObjectFiller;
using WireMock.Server;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.Labs
{
    [Collection(nameof(ApiTestCollection))]
    public partial class LabApiTests
    {
        private readonly DmxCoreApiBroker dmxCoreApiBroker;
        private readonly WireMockServer wireMockServer;

        public LabApiTests(DmxCoreApiBroker dmxCoreApiBroker)
        {
            this.dmxCoreApiBroker = dmxCoreApiBroker;
            this.wireMockServer = WireMockServer.Start(6122);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static bool GetRandomBoolean() => new Random().Next(2) == 1;

        private static List<dynamic> CreateRandomLabsData()
        {
            int randomCount = GetRandomNumber();

            (IDictionary<string, string> randomProperties, List<LabDevice> randomDevices) =
                GetRandomLabProperties();

            var allCases = new List<dynamic>
            {
                new
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = GetRandomString(),
                    IsConnected = true,
                    IsReserved = false,
                    LabStatus = LabStatus.Available,
                    Properties = randomProperties,
                    Devices = randomDevices
                },

                new
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = GetRandomString(),
                    IsConnected = true,
                    IsReserved = true,
                    LabStatus = LabStatus.Reserved,
                    Properties = randomProperties,
                    Devices = randomDevices
                },

                new
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = GetRandomString(),
                    IsConnected = false,
                    IsReserved = GetRandomBoolean(),
                    LabStatus = LabStatus.Offline,
                    Properties = randomProperties,
                    Devices = randomDevices
                }
            };

            return Enumerable.Range(start: 0, count: randomCount)
                .Select(iterator => allCases)
                    .SelectMany(@case => @case)
                        .ToList();
        }

        private static (IDictionary<string, string>, List<LabDevice>) GetRandomLabProperties()
        {
            string randomPhoneName = GetRandomString();
            string randomHMDName = GetRandomString();
            bool randomHostConnectionStatus = GetRandomBoolean();
            bool randomPhoneConnectionStatus = GetRandomBoolean();
            bool randomHMDConnectionStatus = GetRandomBoolean();

            Dictionary<string, string> externalDeviceProperties = new Dictionary<string, string>
            {
                { @"Host\isconnected", $"{randomHostConnectionStatus}" },
                { @"Phone\name", randomPhoneName },
                { @"Phone\isconnected", $"{randomPhoneConnectionStatus}" },
                { @"HMD\name", randomHMDName },
                { @"HMD\isconnected", $"{randomHMDConnectionStatus}" },
            };

            List<LabDevice> labDevices = new List<LabDevice>
            {
                new LabDevice
                {
                    Name = null,
                    Type = LabDeviceType.PC,
                    Category = LabDeviceCategory.Host,

                    Status = randomHostConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },

                new LabDevice
                {
                    Name = randomPhoneName,
                    Type = LabDeviceType.Phone,
                    Category = LabDeviceCategory.Attachment,

                    Status = randomPhoneConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },

                new LabDevice
                {
                    Name = randomHMDName,
                    Type = LabDeviceType.HeadMountedDisplay,
                    Category = LabDeviceCategory.Attachment,

                    Status = randomHMDConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },
            };

            return (externalDeviceProperties, labDevices);
        }
    }
}