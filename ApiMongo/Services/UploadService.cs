using ApiMongo.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace ApiMongo.Services
{
    public class UploadService : IUploadService
    {
        public string UploadFile(IFormFile file)
        {
            var validateTypeMedia = GetTypeMedia(file.FileName);

            return validateTypeMedia.Equals(Media.Image) ? UploadImage(file) : UploadVideo(file);
        }

        private string UploadImage(IFormFile file)
        {
            var urlfile = $"{Guid.NewGuid()}.webp";

            var outputPath = Path.Combine("Medias/Imagens", Path.ChangeExtension(file.FileName, ".webp"));

            using (var image = Image.Load(file.OpenReadStream()))
            {
                image.Save(outputPath, new WebpEncoder
                {
                    Quality = 80 // Tendência de otimização: 80 é o "sweet spot"
                });
            }

            return $"http://localhost:5005/medias/images/{urlfile}";
        }

        private string UploadVideo(IFormFile file)
        {
            FileInfo fi = new FileInfo(file.FileName);

            var fileName = Guid.NewGuid() + fi.Extension;


            using (var stream = new FileStream(Path.Combine("Medias/Videos", fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"http://localhsot:5005/medias/videos/{fileName}";
        }

        public Media GetTypeMedia(string fileName)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".webp" };

            string[] videoExtensions = { ".avi", ".mp4" };

            var fileInfo = new FileInfo(fileName);

            return imageExtensions.Contains(fileInfo.Extension) ? Media.Image : videoExtensions.Contains(fileInfo.Extension) ? Media.Video : throw new DomainException("Formato de imagem inválido!");
        }

    }
}
