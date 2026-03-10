using ApiMongo.Entities;
using MongoDB.Driver;

namespace ApiMongo.Infra
{
    public class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _model;

        public MongoRepository(IDatabaseSetting setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);

            _model = database.GetCollection<T>(typeof(T).Name.ToLower());
        }

        public Result<T> Get(int page, int quantidade)
        {
            var result = new Result<T>
            {
                Page = page,
                Quantidade = quantidade
            };

            var filter = Builders<T>.Filter.Eq(entity => entity.Deleted, false);

            result.Data = _model.Find(filter)
                .SortByDescending(entity => entity.PublishDate)
                .Skip((page - 1) * quantidade)
                .Limit(quantidade).ToList();

            result.Total = _model.CountDocuments(filter);
            result.TotalPage = result.Total / quantidade;

            return result;
        }

        public T Get(string id)
        {
            return _model.Find<T>(news => news.Id == id && !news.Deleted).FirstOrDefault();
        }

        public T GetBySlug(string slug) =>
          _model.Find<T>(news => news.Slug == slug && !news.Deleted).FirstOrDefault();

        public T Create(T news)
        {
            _model.InsertOne(news);

            return news;
        }

        public async Task<T> Update(string id, T newsIn)
        {
            var options = new FindOneAndReplaceOptions<T>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _model.FindOneAndReplaceAsync(news => news.Id == id, newsIn, options: options);
        }

        public void Remove(string id)
        {
            var news = Get(id);

            news.Deleted = true;

            _model.ReplaceOne(news => news.Id == id, news);
        }

      
    }
}
