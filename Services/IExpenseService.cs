using BudgetLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLife.Services
{
    public interface IExpenseService
    {
        List<Expense> GetAll();
        Expense? GetById(int id);
        void Add(Expense expense);
        void Update(Expense expense);
        void Delete(int id);
    }
}
