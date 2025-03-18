namespace DataAccesses.Utils
{
    public class RandomStringGenerator
    {
        private static Random random = new Random();
        private static HashSet<string> generatedStrings = new HashSet<string>();

        public string GenerateUniqueRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomString;

            do
            {
                randomString = new string(Enumerable.Range(0, length)
                    .Select(_ => chars[random.Next(chars.Length)]).ToArray());
            } while (!generatedStrings.Add(randomString)); // Ensure uniqueness

            return randomString;
        }
    }
}
