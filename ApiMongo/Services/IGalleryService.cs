using ApiMongo.Entities;
using ApiMongo.ViewModels;

namespace ApiMongo.Services
{
    public interface IGalleryService
    {
        Result<GalleryViewModel> Get(int page, int quantidade);
        
        GalleryViewModel Get(string id);

        GalleryViewModel GetBySlug(string slug);

        GalleryViewModel Create(GalleryViewModel gallery);

        Task<GalleryViewModel> Update(string id, GalleryViewModel galleryIn);

       void Delete(string id);
    }
}
