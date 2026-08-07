using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetLife.Models;

namespace BudgetLife.Repositories
{
    public interface IExpenseRepository
    {
        Expense? GetById(int id);
        List<Expense> GetAll();
        void Add(Expense expense);
        void Update(Expense expense);
        void Delete(int id);
    }
}
