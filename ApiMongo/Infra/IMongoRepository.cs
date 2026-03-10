using ApiMongo.Entities;
using ApiMongo.ViewModels;
using System.Collections.Generic;

namespace ApiMongo.Infra
{
    public interface IMongoRepository<T>
    {
        Result<T> Get(int page, int quantidade);

        T Get(string id);

        T Create(T news);

        Task<T> Update(string id, T news);
        void Remove(string id);
        T GetBySlug(string slug);
    }
}
