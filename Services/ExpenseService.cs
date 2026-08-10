using BudgetLife.Models;
using BudgetLife.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLife.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ExpenseService(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
        }

        public List<Expense> GetAll()
        {
            return _expenseRepository.GetAll();
        }

        public Expense? GetById(int id)
        {
            return _expenseRepository.GetById(id);
        }

        public void Add(Expense expense)
        {
            if(expense == null)
            {
                throw new ArgumentException("Despesa não deve ser vazia!");
            }
            if (string.IsNullOrWhiteSpace(expense.Name))
            {
                throw new ArgumentException("Despesa deve possuir nome.");
            }
            if (expense.Amount <= 0)
            {
                throw new ArgumentException("Despesa deve possuir valor maior que zero.");
            }
            if (expense.Date == default)
            {
                expense.Date = DateTime.Now;
            }
            if (_categoryRepository.GetById(expense.CategoryId) == null)
            {
                throw new ArgumentException("A categoria informada não existe.");
            }

            _expenseRepository.Add(expense);
        }

        public void Update(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentException("Despesa não deve ser vazia!");
            }
            if (_expenseRepository.GetById(expense.Id) == null) 
            {
                throw new ArgumentException("Despesa não existe.");
            }
            if (string.IsNullOrWhiteSpace(expense.Name))
            {
                throw new ArgumentException("Despesa deve possuir nome.");
            }
            if (expense.Amount <= 0)
            {
                throw new ArgumentException("Despesa deve possuir valor maior que zero.");
            }
            if (expense.Date == default)
            {
                expense.Date = DateTime.Now;
            }
            if (_categoryRepository.GetById(expense.CategoryId) == null)
            {
                throw new ArgumentException("A categoria informada não existe.");
            }

            _expenseRepository.Update(expense);
        }

        public void Delete(int id)
        {
            if (_expenseRepository.GetById(id) == null)
            {
                throw new ArgumentException("A categoria informada não existe.");
            }

            _expenseRepository.Delete(id);
        }
    }
}
