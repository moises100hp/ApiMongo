using ApiMongo.Enums;

namespace ApiMongo.ViewModels
{
    public class VideoViewModel
    {
        public string? Id { get; set; }
        public string Hat { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string ThumbNail { get; set; }
        public string UrlVideo { get; set; }
        public string Slug { get; set; }
        public Status Status { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
