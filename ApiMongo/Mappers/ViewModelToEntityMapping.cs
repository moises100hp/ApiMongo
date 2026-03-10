using ApiMongo.Entities;
using ApiMongo.ViewModels;
using AutoMapper;

namespace ApiMongo.Mappers
{
    public class ViewModelToEntityMapping : Profile
    {
        public ViewModelToEntityMapping()
        {
            #region [Entidade]

            CreateMap<NewsViewModel, News>();
            CreateMap<VideoViewModel, Video>();
            CreateMap<GalleryViewModel, Gallery>();

            #endregion

            #region [Result]

            CreateMap<Result<NewsViewModel>, News>();
            CreateMap<Result<VideoViewModel>, Video>();
            CreateMap<Result<GalleryViewModel>, Gallery>();

            #endregion
        }
    }
}
