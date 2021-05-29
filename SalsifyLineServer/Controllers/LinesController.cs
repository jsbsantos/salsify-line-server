using Microsoft.AspNetCore.Mvc;
using SalsifyLineServer.Services.Interfaces;
using System.Threading.Tasks;

namespace SalsifyLineServer.Controllers
{
    public class LinesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public LinesController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        [Route("lines/{lineIndex}")]
        public async Task<IActionResult> Get(int lineIndex)
        {
            //Get the requested line from the service
            var data = await _fileService.GetLine(lineIndex).ConfigureAwait(false);

            //if it has a value return it - comparing with null to support empty lines 
            if (data != null)
            {
                return Ok(data);
            }

            //if the read line is null (i.e. out of bounds) return a HTTP 413 response
            return Problem(null, null, 413);
        }
    }
}