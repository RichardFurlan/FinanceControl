namespace FinanceControl.Application.DTOs;

public record MonthlyReportDto
{
    public int Year { get; init; }
    public int Month { get; init; }
    public decimal TotalIncome { get; init; }
    public decimal TotalExpenses { get; init; }
    public decimal Balance { get; init; }
    public IEnumerable<CategorySummaryDto> ExpensesByCategory { get; init; } = new List<CategorySummaryDto>();
}

public record CategorySummaryDto
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public decimal Total { get; init; }
    public int TransactionCount { get; init; }
}