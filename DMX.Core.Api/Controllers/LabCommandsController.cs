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
    }
}
