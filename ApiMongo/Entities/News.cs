using ApiMongo.Enums;
using ApiMongo.Infra;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiMongo.Entities
{
    [BsonIgnoreExtraElements]
    public class News : BaseEntity
    {
        public News(string hat, string title, string text, string author, string img, Status status)
        {
            Hat = hat;
            Title = title;
            Text = text;
            Author = author;
            Img = img;
            Slug = Helper.GenerateSlug(Title);
            PublishDate = DateTime.Now;
            Status = status;

            ValidateEntity();
        }

        public Status ChangeStatus(Status status)
        {
            switch (status)
            {
                case Status.Active:
                    status = Status.Active;
                    break;
                case Status.Inactive:
                    status = Status.Inactive;
                    break;
                case Status.Draft:
                    status = Status.Draft;
                    break;
                default:
                    break;
            }

            return Status;
        }
            

        [BsonElement("hat")]
        public string Hat { get; private set; }

        [BsonElement("title")]
        public string Title { get; private set; }

        [BsonElement("text")]
        public string Text { get; private set; }

        [BsonElement("author")]
        public string Author { get; private set; }

        [BsonElement("img")]
        public string Img { get; private set; }

        public void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotEmpty(Title, "O título não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Hat, "O chapéu não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Text, "O texto não pode estar vazio!");
     
            AssertionConcern.AssertArgumentLenght(Title, 90, "O título deve ter até 40 caracteres!");
            AssertionConcern.AssertArgumentLenght(Hat, 40, "O chapéu deve ter até 40 caracteres!");
        }
    }
}
