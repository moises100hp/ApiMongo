using ApiMongo.Entities;

namespace ApiMongo.Testes.Entities
{
    public class NewsTests
    {
        [Fact]
        public void News_Validate_Title_Lenght()
        {
            //Arrange & Act
            var result = Assert.Throws<DomainException>(() => new News(
                "Entretenimento",
                "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs",
                "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs",
                "Da Redação",
                "",
                status: ApiMongo.Enums.Status.Active));

            Assert.Equal("O título deve ter até 40 caracteres!", result.Message);
        }

        [Fact]
        public void News_Validate_Hat_Lenght()
        {
            var result = Assert.Throws<DomainException>(() => new News(
                "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs",
                "A Band preparou uma série de atrações.",
                "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs",
                "Da Redação",
                "",
                status: ApiMongo.Enums.Status.Active));

            Assert.Equal("O chapéu deve ter até 40 caracteres!", result.Message);
        }

        [Fact]
        public void News_validate_Title_Empty()
        {
            var result = Assert.Throws<DomainException>(() => new News(
               "Entretenimento",
                string.Empty,
                "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs",
                "Da Redação",
                "",
                status: ApiMongo.Enums.Status.Active));

            Assert.Equal("O título não pode estar vazio!", result.Message);
        }

        [Fact]
        public void News_Validate_Hat_Empty()
        {
            var result = Assert.Throws<DomainException>(() => new News(
                string.Empty,
                "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs",
                "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs",
                "Da Redação",
                "",
                status: ApiMongo.Enums.Status.Active));

            Assert.Equal("O chapéu não pode estar vazio!", result.Message);
        }

        [Fact]
        public void News_Validate_Description_Empty()
        {
            var result = Assert.Throws<DomainException>(() => new News(
                "Entretenimento",
                "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs",
                string.Empty,
                "Da Redação",
                "",
                status: ApiMongo.Enums.Status.Active));
        }
    }
}
