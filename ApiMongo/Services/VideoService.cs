using ApiMongo.Entities;
using ApiMongo.Infra;
using ApiMongo.Services.IService;
using ApiMongo.ViewModels;
using AutoMapper;

namespace ApiMongo.Services
{
    public class VideoService : IVideoService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository<Video> _video;

        public VideoService(IMapper mapper,
                            IMongoRepository<Video> video)
        {
            _mapper = mapper;
            _video = video;
        }

        public VideoViewModel Create(VideoViewModel video)
        {
            var result = _video.Create(_mapper.Map<Video>(video));

            return _mapper.Map<VideoViewModel>(result);
        }

        public VideoViewModel Get(string id)
        {
            var result = _video.Get(id);

            return _mapper.Map<VideoViewModel>(result);
        }

        public Result<VideoViewModel> GetAll(int page, int quantidade) => 
            _mapper.Map<Result<VideoViewModel>>(_video.Get(page, quantidade));

        public VideoViewModel GetBySlug(string id)
        {
            return _mapper.Map<VideoViewModel>(_video.GetBySlug(id));
        }

        public void Remove(string id) => _video.Remove(id);

        public async Task<VideoViewModel> Update(string id, VideoViewModel videoNew)
        {
            var video = await _video.Update(id, _mapper.Map<Video>(videoNew));

            return _mapper.Map<VideoViewModel>(video);
        }
    }
}
