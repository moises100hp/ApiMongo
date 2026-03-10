namespace ApiMongo.Infra
{
    public static class Helper
    {
        public static string GenerateSlug(string str)
        {
            var acentos = new[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á","É", "Í", "Ó", "Ú", "Ý" };
            
            var semAcentos = new[] { "ç", "Ç", "a", "é", "í", "ó", "ú", "ý", "Á","É", "Í", "Ó", "Ú", "Ý" };

            for (int i = 0; i < acentos.Length; i++)
            {
                str = str.Replace(acentos[i], semAcentos[i]);
            }

            var caracteresEspeciais = new[] { "´" };

            for (int i = 0; i < caracteresEspeciais.Length; i++)
            {
                str.Replace(caracteresEspeciais[i], "");
            }



            return str.Trim().ToLower().Replace("  ", " ").Replace(" ", "-");
        }
    }
}
