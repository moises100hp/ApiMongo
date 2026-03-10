using ApiMongo.Entities;
using ApiMongo.Services;
using ApiMongo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly GalleryService _galleryService;

        public GalleryController(ILogger<GalleryController> logger, 
                                 GalleryService galleryService)
        {
            _logger = logger;
            _galleryService = galleryService;
        }

        [HttpGet("{page}/{quantidade}")]
        public ActionResult<Result<GalleryViewModel>> Get(int page, int quantidade) => _galleryService.Get(page, quantidade);

        [HttpGet("{id:length(24)}", Name = "GetGallery")]
        public ActionResult<GalleryViewModel> Get(string id)
        {
            var news = _galleryService.Get(id);

            if(news is null)
                return NotFound();

            return Ok(news);
        }

        [HttpPost]
        public ActionResult<GalleryViewModel> Create(GalleryViewModel news)
        {
            var result = _galleryService.Create(news);

            return CreatedAtRoute("GetGallery", new { id = result.Id.ToString() }, result);
        }

        [HttpPut]
        public async Task<ActionResult<GalleryViewModel>> Update(string id, GalleryViewModel galleryIn)
        {
            var gallery = _galleryService.Get(id);

            if (gallery is null)
                return NotFound();

            galleryIn.PublishDate = gallery.PublishDate;

            gallery = await _galleryService.Update(id, galleryIn);

            return CreatedAtRoute("GetGallery", new { id = gallery.Id.ToString() }, gallery);
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            var gallery = _galleryService.Get(id);

            if(gallery is null)
                return NotFound();

            _galleryService.Delete(id);

            return Ok();
        }
    }
}
