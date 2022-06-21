// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.ExternalLabs.Exceptions;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace DMX.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalLabsController : RESTFulController
    {
        private readonly IExternalLabService externalLabService;

        public ExternalLabsController(IExternalLabService externalLabService) =>
            this.externalLabService = externalLabService;

        [HttpGet]
        public async ValueTask<ActionResult<List<Lab>>> GetAllLabsAsync()
        {
            try
            {
                List<Lab> allExternalLabs =
                    await this.externalLabService.RetrieveAllLabsAsync();

                return Ok(allExternalLabs);
            }
            catch (ExternalLabDependencyException externalLabDependencyException)
            {
                return InternalServerError(externalLabDependencyException);
            }
            catch (ExternaLabServiceException externalLabServiceException)
            {
                return InternalServerError(externalLabServiceException);
            }
        }
    }
}
