namespace GoodExpense.Users.Domain.Services;

public interface IPasswordEncrypter
{
    public string Encrypt(string password);
}