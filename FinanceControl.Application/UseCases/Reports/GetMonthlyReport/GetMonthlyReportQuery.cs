using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Reports.GetMonthlyReport;

public record GetMonthlyReportQuery(int Year, int Month) : IRequest<ResultViewModel<MonthlyReportDto>>;
