using BudgetLife.Models;
using BudgetLife.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLife.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public List<CategoryTotal> GetTotalByCategory()
        {
            return _reportRepository.GetTotalByCategory();
        }
        public List<MonthlyTotal> GetTotalByMonth(int year)
        {
            return _reportRepository.GetTotalByMonth(year);
        }
        public List<Expense> GetExpensesByPeriod(DateTime start, DateTime end)
        {
            return _reportRepository.GetExpensesByPeriod(start, end);
        }
    }
}
