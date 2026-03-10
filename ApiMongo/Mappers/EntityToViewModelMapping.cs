using ApiMongo.Entities;
using ApiMongo.ViewModels;
using AutoMapper;

namespace ApiMongo.Mappers
{
    public class EntityToViewModelMapping : Profile
    {
        public EntityToViewModelMapping()
        {
            #region [Entidade]

            CreateMap<News, NewsViewModel>();
            CreateMap<Video, VideoViewModel>();
            CreateMap<Gallery, GalleryViewModel>();

            #endregion

            #region [Result]

            CreateMap<Result<News>, Result<NewsViewModel>>();
            CreateMap<Result<Video>, Result<VideoViewModel>>();
            CreateMap<Result<Gallery>, Result<GalleryViewModel>>();

            #endregion
        }
    }
}
