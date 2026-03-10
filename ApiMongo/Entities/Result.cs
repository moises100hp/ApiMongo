using System.Reflection.Metadata.Ecma335;

namespace ApiMongo.Entities
{
    public class Result<T>
    {
        public int Page { get; set; }
        public long Total { get; set; }
        public long TotalPage { get; set; }
        public int Quantidade { get; set; }

        public ICollection<T> Data { get; set; }
    }
}
