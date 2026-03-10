using ApiMongo.Entities;
using ApiMongo.ViewModels;

namespace ApiMongo.Services
{
    public interface IVideoService
    {
        Result<VideoViewModel> GetAll(int page, int quantidade);
        VideoViewModel Get(string id);
        VideoViewModel GetBySlug(string id);
        VideoViewModel Create(VideoViewModel news);
        Task<VideoViewModel> Update(string id, VideoViewModel newsIn);
        void Remove(string id);
    }
}
