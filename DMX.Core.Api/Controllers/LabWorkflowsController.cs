// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using DMX.Core.Api.Services.Foundations.LabWorkflows;
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
    public class LabWorkflowsController : RESTFulController
    {
        private readonly ILabWorkflowService labWorkflowService;

        public LabWorkflowsController(ILabWorkflowService labWorkflowService) =>
            this.labWorkflowService = labWorkflowService;

        [HttpGet("{labWorkflowId}")]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:GetLabWorkflow")]
#endif
        public async ValueTask<ActionResult<LabWorkflow>> GetLabWorkflowByIdAsync(Guid labWorkflowId)
        {
            try
            {
                LabWorkflow labWorkflow =
                    await this.labWorkflowService.RetrieveLabWorkflowByIdAsync(labWorkflowId);

                return Ok(labWorkflow);
            }
            catch (LabWorkflowValidationException labWorkflowValidationException)
                when (labWorkflowValidationException.InnerException is NotFoundLabWorkflowException)
            {
                return NotFound(labWorkflowValidationException.InnerException);
            }
            catch (LabWorkflowValidationException labWorkflowValidationException)
            {
                return BadRequest(labWorkflowValidationException.InnerException);
            }
            catch (LabWorkflowDependencyValidationException labWorkflowDependencyValidationException)
            {
                return BadRequest(labWorkflowDependencyValidationException.InnerException);
            }
            catch (LabWorkflowDependencyException labWorkflowDependencyException)
            {
                return InternalServerError(labWorkflowDependencyException);
            }
            catch (LabWorkflowServiceException labWorkflowServiceException)
            {
                return InternalServerError(labWorkflowServiceException);
            }
        }
    }
}
