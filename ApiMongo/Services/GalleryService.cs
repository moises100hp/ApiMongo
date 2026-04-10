using ApiMongo.Entities;
using ApiMongo.Infra;
using ApiMongo.Services.IService;
using ApiMongo.ViewModels;
using AutoMapper;

namespace ApiMongo.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository<Gallery> _gallery;
        private readonly ICacheService _cacheService;
        private readonly string keyForCache = "gallery";

        public GalleryService(IMapper mapper,
                              IMongoRepository<Gallery> gallery,
                              ICacheService cacheService)
        {
            _mapper = mapper;
            _gallery = gallery;
            _cacheService = cacheService;
        }

        public Result<GalleryViewModel> Get(int page, int quantidade)
        {
            var keyCache = $"{keyForCache}/{page}/{quantidade}";
            var gallery = _cacheService.Get<Result<GalleryViewModel>>(keyCache);

            if(gallery is null)
            {
                gallery = _mapper.Map<Result<GalleryViewModel>>(_gallery.Get(page, quantidade));
                _cacheService.Set(keyCache, gallery);
            }

            return gallery;
        }
            

        public GalleryViewModel Get(string id) =>
            _mapper.Map<GalleryViewModel>(_gallery.Get(id));
        
        public GalleryViewModel GetBySlug(string slug)
        {
            var cacheKey = $"{keyForCache}/{slug}";

            var gallery = _cacheService.Get<GalleryViewModel>(cacheKey);

            if(gallery is null)
            {
                gallery = _mapper.Map<GalleryViewModel>(_gallery.GetBySlug(slug));
                _cacheService.Set(cacheKey, gallery);
            }

            return gallery;
        }
            

        public GalleryViewModel Create(GalleryViewModel gallery)
        {
            var entity = new Gallery(gallery.Title, gallery.Legend, gallery.Author, gallery.Tags, gallery.Status, gallery.GalleryImages, gallery.Thumb, DateTime.Now);
            _gallery.Create(entity);

            var cacheKey = $"{keyForCache}/{entity.Slug}";
            _cacheService.Set(cacheKey, entity);

            return Get(entity.Id);
        }

        public async Task<GalleryViewModel> Update(string id, GalleryViewModel galleryIn)
        {
            var cacheKey = $"{keyForCache}/{id}";
            var gallery = await _gallery.Update(id, _mapper.Map<Gallery>(galleryIn));

            _cacheService.Remove(cacheKey);
            _cacheService.Set(cacheKey, gallery);

            return Get(gallery.Id);
        }

        public void Delete(string id)
        {
            var cacheKey = $"{keyForCache}/{id}";
            _gallery.Remove(cacheKey);

            var gallery = Get(id);

            cacheKey = $"{keyForCache}/{gallery.Slug}";
            _gallery.Remove(cacheKey);

            _cacheService.Remove(cacheKey);
        }
    }
}
