using System.Security.Cryptography;
using System.Text;

namespace CheckerServer.utils
{
    public class HashTool
    {
        public static String hashPassword(string password)
        {
            var sha = SHA256.Create();

            byte[] passAsByteArr = Encoding.Default.GetBytes(password);
            byte[] hashedPsswrd = new byte[1];
            try
            {
                hashedPsswrd = sha.ComputeHash(passAsByteArr);
            }catch(Exception ex){
                Console.WriteLine(ex.Message);
            }

            return Convert.ToBase64String(hashedPsswrd);
        }
    }
}
