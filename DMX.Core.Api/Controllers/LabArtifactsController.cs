// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Http;
using DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions;
using DMX.Core.Api.Services.Foundations.LabArtifacts;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

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
        private readonly ILabArtifactService labArtifactService;

        public LabArtifactsController(ILabArtifactService labArtifactService) =>
            this.labArtifactService = labArtifactService;

        [HttpPost]
#if RELEASE
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:PostLabArtifact")]
#endif
        public async ValueTask<ActionResult<string>> PostLabArtifactAsync([FromUri] string streamName)
        {
            try
            {
                await this.labArtifactService.AddLabArtifactAsync(
                    labArtifactName: streamName,
                    labArtifactContent: Request.Body);

                return Accepted();
            }
            catch (LabArtifactValidationException labArtifactValidationException)
            {
                return BadRequest(labArtifactValidationException.InnerException);
            }
            catch (LabArtifactDependencyException labArtifactDependencyException)
            {
                return InternalServerError(labArtifactDependencyException.InnerException);
            }
            catch (LabArtifactDependencyValidationException labArtifactDependencyValidationException)
                when (labArtifactDependencyValidationException.InnerException is AlreadyExistsLabArtifactException)
            {
                return Conflict(labArtifactDependencyValidationException.InnerException);
            }
            catch (LabArtifactServiceException labArtifactServiceException)
            {
                return InternalServerError(labArtifactServiceException);
            }
        }
    }
}
