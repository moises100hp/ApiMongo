using ApiMongo.Entities;
using ApiMongo.Services;
using FluentAssertions;

namespace ApiMongo.Testes.Services
{
    public class UploadServiceTestes
    {

        [Theory]
        [InlineData(Media.Image, "image.webp")]
        [InlineData(Media.Video, "video.mp4")]
        public void Should_verify_if_Type_is_Image_or_Video(Media media, string filename)
        {
            //Arrange
            var service = new UploadService();

            //Act
            var result = service.GetTypeMedia(filename);

            //Assert
            Assert.Equal(media, result);
        }

        [Theory]
        [InlineData(Media.Image, "image.psd")]
        [InlineData(Media.Video, "video.mp3")]
        public void Should_verify_if_Type_isent_Image_or_Video(Media media, string filename)
        {
            //Arrange
            var service = new UploadService();

            //Act
            var result = () => service.GetTypeMedia(filename);

            //Assert
            result.Should().ThrowExactly<DomainException>().WithMessage("Formato de imagem inválido!");
        }
    }
}
