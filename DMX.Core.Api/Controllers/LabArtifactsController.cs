// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabArtifacts;
using DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions;
using DMX.Core.Api.Services.Foundations.LabArtifacts;
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
    public class LabArtifactsController : RESTFulController
    {
        private readonly ILabArtifactService ILabArtifactService;

        public LabArtifactsController(ILabArtifactService ILabArtifactService) =>
            this.ILabArtifactService = ILabArtifactService;

        [HttpPost]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:PostLabArtifact")]
#endif
        public async ValueTask<ActionResult> PostLabArtifactAsync(LabArtifact labArtifact)
        {
            try
            {
                await this.ILabArtifactService.AddLabArtifactAsync(labArtifact);

                return Created(labArtifact);
            }
            catch (LabArtifactValidationException labArtifactValidationException)
            {
                return BadRequest(labArtifactValidationException.InnerException);
            }
            catch (LabArtifactDependencyException labArtifactDependencyException)
            {
                return InternalServerError(labArtifactDependencyException.InnerException);
            }
            catch (LabArtifactServiceException labArtifactServiceException)
            {
                return InternalServerError(labArtifactServiceException);
            }
        }
    }
}
