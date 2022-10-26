// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using DMX.Core.Api.Services.Foundations.LabWorkflowCommands;
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
    public class LabWorkflowCommandsController : RESTFulController
    {
        private readonly ILabWorkflowCommandService labWorkflowCommandService;

        public LabWorkflowCommandsController(ILabWorkflowCommandService labWorkflowCommandService) =>
            this.labWorkflowCommandService = labWorkflowCommandService;

        [HttpPost]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:PostLabWorkflowCommand")]
#endif
        public async ValueTask<ActionResult<LabWorkflowCommand>> PostLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand)
        {
            try
            {
                LabWorkflowCommand addedLabWorkflowCommand =
                    await this.labWorkflowCommandService.AddLabWorkflowCommandAsync(labWorkflowCommand);

                return Created(addedLabWorkflowCommand);
            }
            catch (LabWorkflowCommandValidationException labWorkflowCommandValidationException)
            {
                return BadRequest(labWorkflowCommandValidationException.InnerException);
            }
            catch (LabWorkflowCommandDependencyValidationException labWorkflowCommandDependencyValidationException)
                when (labWorkflowCommandDependencyValidationException.InnerException is AlreadyExistsLabWorkflowCommandException)
            {
                return Conflict(labWorkflowCommandDependencyValidationException.InnerException);
            }
            catch (LabWorkflowCommandDependencyValidationException labWorkflowCommandDependencyValidationException)
            {
                return BadRequest(labWorkflowCommandDependencyValidationException.InnerException);
            }
            catch (LabWorkflowCommandDependencyException labWorkflowCommandDependencyException)
            {
                return InternalServerError(labWorkflowCommandDependencyException.InnerException);
            }
            catch (LabWorkflowCommandServiceException labWorkflowCommandServiceException)
            {
                return InternalServerError(labWorkflowCommandServiceException);
            }
        }

        [HttpPut]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:PutLabWorkflowCommand")]
#endif
        public async ValueTask<ActionResult<LabWorkflowCommand>> PutLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand)
        {
            try
            {
                LabWorkflowCommand modifiedLabWorkflowCommand =
                    await this.labWorkflowCommandService.ModifyLabWorkflowCommand(labWorkflowCommand);

                return Created(modifiedLabWorkflowCommand);
            }
            catch (LabWorkflowCommandValidationException labWorkflowCommandValidationException)
                when (labWorkflowCommandValidationException.InnerException is NotFoundLabWorkflowCommandException)
            {
                return NotFound(labWorkflowCommandValidationException.InnerException);
            }
            catch (LabWorkflowCommandValidationException labWorkflowCommandValidationException)
            {
                return BadRequest(labWorkflowCommandValidationException.InnerException);
            }
            catch (LabWorkflowCommandDependencyValidationException labWorkflowCommandDependencyValidationException)
                when (labWorkflowCommandDependencyValidationException.InnerException is LockedLabWorkflowCommandException)
            {
                return Locked(labWorkflowCommandDependencyValidationException.InnerException);
            }
            catch (LabWorkflowCommandDependencyValidationException labWorkflowCommandDependencyValidationException)
            {
                return BadRequest(labWorkflowCommandDependencyValidationException.InnerException);
            }
            catch (LabWorkflowCommandDependencyException labWorkflowCommandDependencyException)
            {
                return InternalServerError(labWorkflowCommandDependencyException.InnerException);
            }
            catch (LabWorkflowCommandServiceException labWorkflowCommandServiceException)
            {
                return InternalServerError(labWorkflowCommandServiceException);
            }
        }
    }
}
