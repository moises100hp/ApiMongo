using ApiMongo.Entities;
using ApiMongo.Infra;
using ApiMongo.ViewModels;
using AutoMapper;

namespace ApiMongo.Services
{
    public class NewsService : INewsService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository<News> _news;

        public NewsService(IMapper mapper, IMongoRepository<News> news)
        {
            _mapper = mapper;
            _news = news;
        }

        public Result<NewsViewModel> GetAll(int page, int quantidade) =>
            _mapper.Map<Result<NewsViewModel>>(_news.Get(page, quantidade));

        public NewsViewModel Get(string id) =>
            _mapper.Map<NewsViewModel>(_news.Get(id));

        public NewsViewModel GetBySlug(string slug) =>
            _mapper.Map<NewsViewModel>(_news.GetBySlug(slug));

        public NewsViewModel Create(NewsViewModel news)
        {
            var entity = new News(news.Hat, news.Title, news.Text, news.Author, news.Img, news.Status);

            _news.Create(entity);
            return Get(entity.Id);
        }

        public async Task<NewsViewModel> Update(string id, NewsViewModel newsIn)
        {
           return _mapper.Map<NewsViewModel>(await _news.Update(id, _mapper.Map<News>(newsIn)));
        }

        public void Remove(string id) => _news.Remove(id);
    }
}
