﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using DMX.Core.Api.Services.Foundations.Labs;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace DMX.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabsController : RESTFulController
    {
        private readonly IExternalLabService labService;

        public LabsController(IExternalLabService labService) =>
            this.labService = labService;

        [HttpGet]
        public async ValueTask<ActionResult<List<Lab>>> GetAllLabsAsync()
        {
            try
            {
                List<Lab> allLabs =
                    await this.labService.RetrieveAllLabsAsync();

                return Ok(allLabs);
            }
            catch (LabDependencyException labDependencyException)
            {
                return InternalServerError(labDependencyException);
            }
            catch (LabServiceException labServiceException)
            {
                return InternalServerError(labServiceException);
            }
        }
    }
}
