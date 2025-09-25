namespace BankApp.Services
{
    public class GenerateIBAN
    {
        private static readonly Random _random = new Random();
        public static string Generate()
        {
            long randomNumber = _random.Next(100000000, 999999999);
            string iban = "RO" + "SBK" + randomNumber.ToString("D13");
            int checksum = (int)(randomNumber % 100);
            iban += checksum.ToString("D2");
            return iban;
        }
    }
}
