using ApiMongo.Entities;
using ApiMongo.Infra;
using ApiMongo.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiMongo.Services
{
    public class NewsService : INewsService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository<News> _news;
        private readonly ICacheService _cacheService;
        private readonly string keyForCache = "news";

        public NewsService(IMapper mapper,
                           IMongoRepository<News> news,
                           ICacheService cacheService)
        {
            _mapper = mapper;
            _news = news;
            _cacheService = cacheService;
        }

        public Result<NewsViewModel> GetAll(int page, int quantidade)
        {
            var keyCache = $"{keyForCache}/{page}/{quantidade}";
            var news = _cacheService.Get<Result<NewsViewModel>>(keyForCache);

            if (news is null)
            {
                news = _mapper.Map<Result<NewsViewModel>>(_news.Get(page, quantidade));
                _cacheService.Set(keyForCache, news);
            }

            return news;
        }

        public NewsViewModel Get(string id)
        {
            var keyCache = $"{keyForCache}/{id}";
            var news = _cacheService.Get<NewsViewModel>(keyForCache);

            if (news is null)
            {
                news = _mapper.Map<NewsViewModel>(_news.Get(id));
                _cacheService.Set(keyCache, news);
            }

            return news;
        }

        public NewsViewModel GetBySlug(string slug)
        {
            var keyCache = $"{keyForCache}/{slug}";
            var news = _cacheService.Get<NewsViewModel>(keyForCache);

            if (news is null)
            {
                news = _mapper.Map<NewsViewModel>(_news.GetBySlug(slug));
                _cacheService.Set(slug, news);
            }

            return news;
        }


        public NewsViewModel Create(NewsViewModel news)
        {
            var entity = new News(news.Hat, news.Title, news.Text, news.Author, news.Img, news.Status);
            _news.Create(entity);

            var cacheKey = $"{keyForCache}/{entity.Id}";
            _cacheService.Set(cacheKey, entity);

            return Get(entity.Id);
        }

        public async Task<NewsViewModel> Update(string id, NewsViewModel newsIn)
        {
            var cacheKey = $"{keyForCache}/{id}";
            var news = _mapper.Map<NewsViewModel>(await _news.Update(id, _mapper.Map<News>(newsIn)));

            _cacheService.Remove(cacheKey);
            _cacheService.Set(cacheKey, news);

            return Get(news.Id);
        }

        public void Remove(string id)
        {
            var cacheKey = $"{keyForCache}/{id}";
            _news.Remove(id);

            var news = Get(id);

             cacheKey = $"{keyForCache}/{news.Slug}";
            _news.Remove(cacheKey);

            _cacheService.Remove(cacheKey);
        }
    }
}
