using BudgetLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLife.Services
{
    public interface IReportService
    {
        List<CategoryTotal> GetTotalByCategory();
        List<MonthlyTotal> GetTotalByMonth(int year);
        List<Expense> GetExpensesByPeriod(DateTime start, DateTime end);
    }
}
