// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs.Exceptions;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations.Labs;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System.Reflection.Metadata;

namespace DMX.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabsController : RESTFulController
    {
        private readonly ILabService labService;

        public LabsController(ILabService externalLabService) =>
            this.labService = externalLabService;

        [HttpPost]
        public async ValueTask<ActionResult<Lab>> PostLabAsync(Lab lab)
        {
            try
            {
                Lab addedLab =
                    await this.labService.AddLabAsync(lab);

                return Created(addedLab);
            }
            catch (LabValidationException labValidationException)
            {
                return BadRequest(labValidationException.InnerException);
            }
            catch (LabDependencyException labDependencyException)
            {
                return InternalServerError(labDependencyException);
            }
            catch (LabDependencyValidationException labDependencyValidationException)
                when (labDependencyValidationException.InnerException is AlreadyExistsLabException)
            {
                return Conflict(labDependencyValidationException.InnerException);
            }
            catch (LabDependencyValidationException labDependencyValidationException)
            {
                return BadRequest(labDependencyValidationException.InnerException);
            }
            catch(LabServiceException labServiceException)
            {
                return InternalServerError(labServiceException);
            }
        }
    }
}
