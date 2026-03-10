using ApiMongo.Enums;
using ApiMongo.Infra;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiMongo.Entities
{
    public class Gallery : BaseEntity
    {
        public Gallery(string title, string legend, string author, string tags, Status status, IList<String> galleryImages, string thumb,  DateTime? publishDate = null)
        {
            Title = title;
            Legend = legend;
            Author = author;
            Tags = tags;
            Slug = Helper.GenerateSlug(Title);
            Status = status;
            GalleryImages = galleryImages;
            this.PublishDate = publishDate ?? DateTime.Now;

            ValidateEntity();
        }

        [BsonElement("title")]
        public string Title { get; private set; }

        [BsonElement("legend")]
        public string Legend { get; private set; }

        [BsonElement("author")]
        public string Author { get; private set; }

        [BsonElement("tags")]
        public string Tags { get; private set; }

        [BsonElement("Thumb")]
        public string Thumb { get; private set; }

        [BsonElement("galleryImages")]
        public IList<string> GalleryImages { get; private set; }


        public void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotEmpty(Title, "O título não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Legend, "A legenda não pode estar vazia!");

            AssertionConcern.AssertArgumentLenght(Title, 90, "O título deve ter até 90 caractéres!");
            AssertionConcern.AssertArgumentLenght(Legend, 40, "A legenda deve ter até 40 caractéres!");
        }
    }
}
