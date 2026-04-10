using ApiMongo.Services.IService;
using ImageProcessor;
using ImageProcessorCore.Plugins.WebP.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace ApiMongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IUploadService _uploadService;

        public UploadController(ILogger<UploadController> logger,
                                IUploadService uploadService)
        {
            _logger = logger;
            _uploadService = uploadService;
        }

        [HttpPost]
        public IActionResult Post(IFormFile file)
        {
            try
            {
                if (file == null) return null;


                var urlFile = _uploadService.UploadFile(file);

                return Ok(new
                {
                    mensagem = "Arquivo salvo com sucesso!",
                    urlImagem = urlFile
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro no upload: " + ex.Message);
            }
        }
    }
}
