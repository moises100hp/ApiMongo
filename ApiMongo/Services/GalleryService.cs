using ApiMongo.Entities;
using ApiMongo.Infra;
using ApiMongo.ViewModels;
using AutoMapper;

namespace ApiMongo.Services
{
    public class GalleryService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository<Gallery> _gallery;

        public GalleryService(IMapper mapper, 
                              IMongoRepository<Gallery> gallery)
        {
            _mapper = mapper;
            _gallery = gallery;
        }

        public Result<GalleryViewModel> Get(int page, int quantidade) => 
            _mapper.Map<Result<GalleryViewModel>>(_gallery.Get(page, quantidade));

        public GalleryViewModel Get(string id) =>
            _mapper.Map<GalleryViewModel>(_gallery.GetBySlug(id));

        public GalleryViewModel Create(GalleryViewModel gallery)
        {
            var entity = new Gallery(gallery.Title, gallery.Legend, gallery.Author, gallery.Tags, gallery.Status, gallery.GalleryImages, gallery.Thumb, DateTime.Now);

            _gallery.Create(entity);

            return Get(entity.Id);
        }

        internal async Task<GalleryViewModel> Update(string id, GalleryViewModel galleryIn)
        {
            var gallery = await _gallery.Update(id, _mapper.Map<Gallery>(galleryIn));

            return Get(gallery.Id);
        }

        internal void Delete(string id)
        {
            _gallery.Remove(id);
        }
    }
}
