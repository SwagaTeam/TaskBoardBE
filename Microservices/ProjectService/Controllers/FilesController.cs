using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ProjectModels;
using SharedLibrary.ProjectModels.File;
using System.IO;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("file")]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment appEnvironment;

        public FilesController(IWebHostEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("upload")]
        public async Task<IActionResult> Upload(IFormFile uploadedFile)
        {
            try
            {
                if (uploadedFile != null)
                {
                    //TODO: вынести логику
                    string path = "/Files/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                    //_context.Files.Add(file);
                    //_context.SaveChanges();

                    return Ok(file.Id);
                }

                return BadRequest("uploadedFile is null");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [ProducesResponseType<FileModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //логика
            FileModel file = new FileModel();
            return Ok(file); // возвращает айди
        }
    }
}
