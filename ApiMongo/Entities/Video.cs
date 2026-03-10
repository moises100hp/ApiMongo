using ApiMongo.Enums;
using ApiMongo.Infra;
using MongoDB.Bson.Serialization.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace ApiMongo.Entities
{
    public class Video : BaseEntity
    {
        public Video(string hat, string title, string author, string thumbnail, string urlVideo, Status status)
        {
            Hat = hat;
            Title = title;
            Author = author;
            Thumbnail = thumbnail;
            UrlVideo = urlVideo;
            Slug = Helper.GenerateSlug(Title);
            PublishDate = DateTime.Now;
            Status = status;

            ValidateEntity();
        }

        [BsonElement("hat")]
        public string Hat { get; private set; }

        [BsonElement("title")]
        public string Title { get; private set; }

        [BsonElement("author")]
        public string Author { get; private set; }

        [BsonElement("thumbnail")]
        public string Thumbnail { get; private set; }

        [BsonElement("urlVideo")]
        public string UrlVideo { get; private set; }

        public void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotEmpty(Title, "O título não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Hat, "O chapéu não pode estar vazio!");

            AssertionConcern.AssertArgumentLenght(Title, 90, "O título deve ter até 40 caracteres!");
            AssertionConcern.AssertArgumentLenght(Hat, 40, "O chapéu deve ter até 40 caracteres!");
        }
    }
}
