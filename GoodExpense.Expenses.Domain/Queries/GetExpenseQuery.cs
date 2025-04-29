using GoodExpense.Expenses.Domain.Dto;
using MediatR;

namespace GoodExpense.Expenses.Domain.Queries;

public class GetExpenseQuery : IRequest<GetExpenseDto?>
{
    public int Id { get; set; }
}