using ApiMongo.Infra;

namespace ApiMongo.Testes.Infra
{
    public class HelperTests
    {
        [Fact]
        public void Should_return_Validate_Slug()
        {
            //Arrange
            var title = "A Band preparou uma série de atrações para agitar o fim de ano. Nesta terça-feira (21), as 21hs";

            //Act
            var slug = Helper.GenerateSlug(title);

            //Assert
            Assert.Equal("a-band-preparou-uma-série-de-atrações-para-agitar-o-fim-de-ano.-nesta-terça-feira-(21),-as-21hs", slug);
        }
    }
}
