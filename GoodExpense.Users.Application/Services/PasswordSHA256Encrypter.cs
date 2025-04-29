using System.Security.Cryptography;
using System.Text;
using GoodExpense.Users.Domain.Services;

namespace GoodExpense.Users.Application.Services;

public class PasswordSHA256Encrypter : IPasswordEncrypter
{
    public string Encrypt(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);

        return Convert.ToBase64String(hash);
    }
}