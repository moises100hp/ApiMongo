using ApiMongo.Entities;

namespace ApiMongo.Services.IService
{
    public interface IUploadService
    {
        string UploadFile(IFormFile file);
    }
}
