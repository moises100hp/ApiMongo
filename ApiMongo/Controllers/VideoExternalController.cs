using ApiMongo.Entities;
using ApiMongo.Services.IService;
using ApiMongo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class VideoExternalController : ControllerBase
    {
        [ApiController]
        [Route("[controller]")]
        public class VideoController : ControllerBase
        {
            private readonly ILogger<VideoExternalController> _logger;
            private readonly IVideoService _videoService;

            public VideoController(ILogger<VideoExternalController> logger, IVideoService videoService)
            {
                _logger = logger;
                _videoService = videoService;
            }

            [HttpGet]
            public ActionResult<Result<VideoViewModel>> Get(int page, int quantidade) => _videoService.GetAll(page, quantidade);

            [HttpGet("{id:length(24)}", Name = "GetVideo")]
            public ActionResult<VideoViewModel> Get(string id)
            {
                var videos = _videoService.Get(id);

                if (videos is null)
                    return NotFound();

                return videos;
            }

            [HttpPost]
            public ActionResult<VideoViewModel> Create(VideoViewModel video)
            {
                var result = _videoService.Create(video);

                return CreatedAtRoute("GetVideo", new { id = result.Id.ToString() }, result);
            }

            [HttpPut("{id:length(24)}")]
            public ActionResult<VideoViewModel> Update(string id, VideoViewModel video)
            {
                var result = _videoService.Get(id);

                if (result is null)
                    return NotFound();

                _videoService.Update(id, video);

                return CreatedAtRoute("GetVideo", new { id = result.Id.ToString() }, result);
            }

            [HttpDelete("{id:length(24)}")]
            public IActionResult Delete(string id)
            {
                var video = _videoService.Get(id);

                if (video is null)
                    return NotFound();

                _videoService.Remove(id);

                var result = new
                {
                    message = "Video deletado com sucesso!"
                };

                return Ok(result);
            }
        }
    }
}
