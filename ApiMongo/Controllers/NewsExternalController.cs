using ApiMongo.Entities;
using ApiMongo.Services;
using ApiMongo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class NewsExternalController : ControllerBase
    {
        private readonly ILogger<NewsExternalController> _logger;
        private readonly INewsService _newsService;

        public NewsExternalController(ILogger<NewsExternalController> logger, 
                                      INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        [HttpGet]
        public ActionResult<Result<NewsViewModel>> Get(int page, int quantidade) => _newsService.GetAll(page, quantidade);

        [HttpGet("{slug}")]
        public ActionResult<NewsViewModel> Get(string slug)
        {
            var news = _newsService.GetBySlug(slug);

            if(news is null)
                return NotFound();

            return Ok(news);
        }
    }
}
