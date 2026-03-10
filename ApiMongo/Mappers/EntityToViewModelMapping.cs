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

            #endregion

            #region [Result]

            CreateMap<Result<News>, Result<NewsViewModel>>();
            CreateMap<Result<Video>, Result<VideoViewModel>>();

            #endregion
        }
    }
}
