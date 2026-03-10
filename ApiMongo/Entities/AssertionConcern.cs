namespace ApiMongo.Entities
{
    public class AssertionConcern
    {
        /// <summary>
        /// validação de tamamnho máximo de string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="maximum"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentLenght(string stringValue, int maximum, string message)
        {
            int lenght = stringValue.Trim().Length;

            if (lenght > maximum)
                throw new DomainException(message);
        }

        public static void AssertArgumentLenght(string stringValue, int minimum, int maximum, string message)
        {
            int lenght = stringValue.Trim().Length;

            if (lenght < minimum || lenght > maximum)
                throw new DomainException(message);
        }

        /// <summary>
        /// Validação de string se esta vazia
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="message"></param>
        public static void AssertArgumentNotEmpty(string stringValue, string message)
        {
            if (stringValue == null || stringValue.Trim().Length == 0)
            {
                throw new DomainException(message);
            }
        }

        /// <summary>
        /// Validação se objeto é null
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="message"></param>
        public static void AssertArgumentNotNull(object object1, string message)
        {
            if (object1 == null)
            {
                throw new DomainException(message);
            }
        }
    }
}
