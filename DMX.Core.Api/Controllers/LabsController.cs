// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using DMX.Core.Api.Services.Orchestrations.Labs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

#if RELEASE
using Microsoft.Identity.Web.Resource;
#endif

namespace DMX.Core.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LabsController : RESTFulController
    {
        private readonly ILabOrchestrationService labOrchestrationService;

        public LabsController(ILabOrchestrationService labOrchestrationService) =>
            this.labOrchestrationService = labOrchestrationService;

        [HttpPost]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:PostLab")]
#endif
        public async ValueTask<ActionResult<Lab>> PostLabAsync(Lab lab)
        {
            try
            {
                Lab addedLab =
                    await this.labOrchestrationService.AddLabAsync(lab);

                return Created(addedLab);
            }
            catch (LabOrchestrationValidationException labOrchestrationValidationException)
            {
                return BadRequest(labOrchestrationValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyValidationException labOrchestrationDependencyValidationException)
                when (labOrchestrationDependencyValidationException.InnerException is AlreadyExistsLabException)
            {
                return Conflict(labOrchestrationDependencyValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyValidationException labOrchestrationDependencyValidationException)
            {
                return BadRequest(labOrchestrationDependencyValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyException labOrchestrationDependencyException)
            {
                return InternalServerError(labOrchestrationDependencyException.InnerException);
            }
            catch (LabOrchestrationServiceException labOrchestrationServiceException)
            {
                return InternalServerError(labOrchestrationServiceException);
            }
        }

        [HttpGet]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:GetAllLabs")]
#endif
        public async ValueTask<ActionResult<List<Lab>>> GetAllLabsAsync()
        {
            try
            {
                List<Lab> allLabs =
                    await this.labOrchestrationService.RetrieveAllLabsAsync();

                return Ok(allLabs);
            }
            catch (LabOrchestrationDependencyException labOrchestrationDependencyException)
            {
                return InternalServerError(labOrchestrationDependencyException);
            }
            catch (LabOrchestrationServiceException labOrchestrationServiceException)
            {
                return InternalServerError(labOrchestrationServiceException);
            }
        }

        [HttpGet("{labId}")]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:GetAllLabs")]
#endif
        public async ValueTask<ActionResult<Lab>> GetLabByIdAsync(Guid labId)
        {
            try
            {
                Lab lab =
                    await this.labOrchestrationService.RetrieveLabByIdAsync(labId);

                return Ok(lab);
            }
            catch (LabOrchestrationValidationException labOrchestrationValidationException)
            {
                return BadRequest(labOrchestrationValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyValidationException labOrchestrationDependencyValidationException)
                when (labOrchestrationDependencyValidationException.InnerException is NotFoundLabException)
            {
                return NotFound(labOrchestrationDependencyValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyValidationException labOrchestrationDependencyValidationException)
            {
                return BadRequest(labOrchestrationDependencyValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyException labOrchestrationDependencyException)
            {
                return InternalServerError(labOrchestrationDependencyException);
            }
            catch (LabOrchestrationServiceException labOrchestrationServiceException)
            {
                return InternalServerError(labOrchestrationServiceException);
            }
        }

        [HttpDelete("{labId}")]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:DeleteLab")] 
#endif
        public async ValueTask<ActionResult<Lab>> DeleteLabByIdAsync(Guid labId)
        {
            try
            {
                Lab removedLab =
                    await this.labOrchestrationService.RemoveLabByIdAsync(labId);

                return Ok(removedLab);
            }
            catch (LabOrchestrationValidationException labOrchestrationValidationException)
            {
                return BadRequest(labOrchestrationValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyValidationException labOrchestrationDependencyValidationException)
                when (labOrchestrationDependencyValidationException.InnerException is NotFoundLabException)
            {
                return NotFound(labOrchestrationDependencyValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyValidationException labOrchestrationDependencyValidationException)
                when (labOrchestrationDependencyValidationException.InnerException is LockedLabException)
            {
                return Locked(labOrchestrationDependencyValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyValidationException labOrchestrationDependencyValidationException)
            {
                return BadRequest(labOrchestrationDependencyValidationException.InnerException);
            }
            catch (LabOrchestrationDependencyException labOrchestrationDependencyException)
            {
                return InternalServerError(labOrchestrationDependencyException);
            }
            catch (LabOrchestrationServiceException labOrchestrationServiceException)
            {
                return InternalServerError(labOrchestrationServiceException);
            }
        }
    }
}
