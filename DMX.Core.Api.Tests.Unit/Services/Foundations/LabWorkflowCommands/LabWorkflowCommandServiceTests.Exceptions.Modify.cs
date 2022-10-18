// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            LabWorkflowCommand someLabWorkflowCommand = CreateRandomLabWorkflowCommand();
            SqlException sqlException = GetSqlException();

            // when

            // then
        }
    }
}
