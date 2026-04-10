using ApiMongo.Entities;
using ApiMongo.ViewModels;

namespace ApiMongo.Services.IService
{
    public interface INewsService
    {
        Result<NewsViewModel> GetAll(int page, int quantidade);
        NewsViewModel Get(string id);
        NewsViewModel GetBySlug(string id);
        NewsViewModel Create(NewsViewModel news);
        Task<NewsViewModel> Update(string id, NewsViewModel newsIn);
        void Remove(string id);
    }
}
