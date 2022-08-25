// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using DMX.Core.Api.Services.Foundations.LabCommands;
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
    public class LabCommandsController : RESTFulController
    {
        private readonly ILabCommandService labCommandService;

        public LabCommandsController(ILabCommandService labCommandService) =>
            this.labCommandService = labCommandService;

        [HttpPost]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:PostLabCommand")]
#endif
        public async ValueTask<ActionResult<LabCommand>> PostLabCommandAsync(LabCommand labCommand)
        {
            try
            {
                LabCommand addedLabCommand =
                    await this.labCommandService.AddLabCommandAsync(labCommand);

                return Created(addedLabCommand);
            }
            catch (LabCommandValidationException labCommandValidationException)
            {
                return BadRequest(labCommandValidationException.InnerException);
            }
            catch (LabCommandDependencyException labCommandDependencyException)
            {
                return InternalServerError(labCommandDependencyException);
            }
            catch (LabCommandDependencyValidationException labCommandDependencyValidationException)
                when (labCommandDependencyValidationException.InnerException is AlreadyExistsLabCommandException)
            {
                return Conflict(labCommandDependencyValidationException.InnerException);
            }
            catch (LabCommandDependencyValidationException labCommandDependencyValidationException)
            {
                return BadRequest(labCommandDependencyValidationException.InnerException);
            }
            catch (LabCommandServiceException labCommandServiceException)
            {
                return InternalServerError(labCommandServiceException);
            }
        }

        [HttpGet("{labCommandId}")]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:GetLabCommand")]
#endif
        public async ValueTask<ActionResult<LabCommand>> GetLabCommandByIdAsync(Guid labCommandId)
        {
            try
            {
                LabCommand labCommand =
                    await this.labCommandService.RetrieveLabCommandByIdAsync(labCommandId);

                return Ok(labCommand);
            }
            catch (LabCommandValidationException labCommandValidationException)
                when (labCommandValidationException.InnerException is NotFoundLabCommandException)
            {
                return NotFound(labCommandValidationException.InnerException);
            }
            catch (LabCommandValidationException labCommandValidationException)
            {
                return BadRequest(labCommandValidationException.InnerException);
            }
            catch (LabCommandDependencyValidationException labCommandDependencyValidationException)
            {
                return BadRequest(labCommandDependencyValidationException.InnerException);
            }
            catch (LabCommandDependencyException labCommandDependencyException)
            {
                return InternalServerError(labCommandDependencyException);
            }
            catch (LabCommandServiceException labCommandServiceException)
            {
                return InternalServerError(labCommandServiceException);
            }
        }

        [HttpPut]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:PutLabCommand")]
#endif
        public async ValueTask<ActionResult<LabCommand>> PutLabCommandAsync(LabCommand labCommand)
        {
            try
            {
                LabCommand modifiedLabCommand =
                    await this.labCommandService.ModifyLabCommandAsync(labCommand);

                return Ok(modifiedLabCommand);
            }
            catch (LabCommandValidationException labCommandValidationException)
                when (labCommandValidationException.InnerException is NotFoundLabCommandException)
            {
                return NotFound(labCommandValidationException.InnerException);
            }
            catch (LabCommandValidationException labCommandValidationException)
            {
                return BadRequest(labCommandValidationException.InnerException);
            }
            catch (LabCommandDependencyValidationException labCommandDependencyValidationException)
                when (labCommandDependencyValidationException.InnerException is LockedLabCommandException)
            {
                return Locked(labCommandDependencyValidationException.InnerException);
            }
            catch (LabCommandDependencyValidationException labCommandDependencyValidationException)
            {
                return BadRequest(labCommandDependencyValidationException.InnerException);
            }
            catch (LabCommandDependencyException labCommandDependencyException)
            {
                return InternalServerError(labCommandDependencyException);
            }
            catch (LabCommandServiceException labCommandServiceException)
            {
                return InternalServerError(labCommandServiceException);
            }
        }
    }
}
