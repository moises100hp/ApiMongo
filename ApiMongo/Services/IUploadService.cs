using ApiMongo.Entities;

namespace ApiMongo.Services
{
    public interface IUploadService
    {
        string UploadFile(IFormFile file);
    }
}
