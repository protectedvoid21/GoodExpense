namespace GoodExpense.Common.Domain.Dto;

public class RegisterUserDto
{
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}