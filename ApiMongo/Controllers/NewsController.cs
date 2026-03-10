using ApiMongo.Entities;
using ApiMongo.Services;
using ApiMongo.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsService _newsService;

        public NewsController(ILogger<NewsController> logger,
                              INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        [HttpGet]
        public ActionResult<Result<NewsViewModel>> Get(int page, int quantidade) => _newsService.GetAll(page, quantidade);

        [HttpGet("{id:length(24)}", Name = "GetNews")]
        public ActionResult<NewsViewModel> Get(string id)
        {
            var news = _newsService.Get(id);

            if (news is null)
                return NotFound();

            return news;
        }

        [HttpPost]
        public ActionResult<NewsViewModel> Create(NewsViewModel news)
        {
            var result = _newsService.Create(news);
            return CreatedAtRoute("GetNews", new { id = result.Id.ToString() }, result);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, NewsViewModel newsIn)
        {
            var news = _newsService.Get(id);

            if (news is null)
                return NotFound();

            news = await _newsService.Update(id, newsIn);

            return CreatedAtRoute("GetNews", new { id = news.Id.ToString() }, news);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var news = _newsService.Get(id);

            if (news is null)
                return NotFound();

            _newsService.Remove(news.Id);

            return Ok("Notícia deletada com sucesso!");
        }
    }
}
