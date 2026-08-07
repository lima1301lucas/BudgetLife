using BudgetLife.Models;
using BudgetLife.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLife.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository, IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category? GetById(int id) 
        {
            return _categoryRepository.GetById(id);
        }

        public void Add(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Categoria deve possuir nome.");
            }

            _categoryRepository.Add(category);
        }

        public void Update(Category category) 
        {
            if (_categoryRepository.GetById(category.Id) == null)
            {
                throw new ArgumentException("Categoria não existe.");
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Categoria deve possuir nome.");
            }

            _categoryRepository.Update(category);
        }

        public void Delete(int id)
        {
            if (_categoryRepository.GetById(id) == null)
            {
                throw new ArgumentException("A categoria informada não existe.");
            }

            if (_expenseRepository.ExistsByCategoryId(id))
            {
                throw new ArgumentException("Não é possível excluir: existem despesas vinculadas a essa categoria.");
            }

            _categoryRepository.Delete(id);
        }
    }
}
