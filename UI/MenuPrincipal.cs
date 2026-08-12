using BudgetLife.Models;
using BudgetLife.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetLife.UI
{
    public class MenuPrincipal
    {
        private readonly IExpenseService _expenseService;
        private readonly ICategoryService _categoryService;
        private readonly IReportService _reportService;

        public MenuPrincipal(IExpenseService expenseService, ICategoryService categoryService, IReportService reportService)
        {
            _expenseService = expenseService;
            _categoryService = categoryService;
            _reportService = reportService;
        }

        public void Init()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== BudgetLife ===");
                Console.WriteLine("1. Listar despesas");
                Console.WriteLine("2. Cadastrar despesa");
                Console.WriteLine("3. Editar despesa");
                Console.WriteLine("4. Excluir despesa");
                Console.WriteLine("5. Listar categorias");
                Console.WriteLine("6. Cadastrar categoria");
                Console.WriteLine("7. Editar categoria");
                Console.WriteLine("8. Excluir categoria");
                Console.WriteLine("9. Relatório: Total por categoria");
                Console.WriteLine("10. Relatório: Total por mês");
                Console.WriteLine("11. Relatório: Despesas por período");
                Console.WriteLine("0. Sair");
                Console.Write("Escolha uma opção: ");

                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListExpenses();                        
                        break;
                    case "2":
                        RegisterExpense();
                        break;
                    case "3":
                        EditExpense();
                        break;
                    case "4":
                        DeleteExpense();
                        break;
                    case "5":
                        ListCategory();
                        break;
                    case "6":
                        RegisterCategory();
                        break;
                    case "7":
                        EditCategory();
                        break;
                    case "8":
                        DeleteCategory();
                        break;
                    case "9":
                        ShowTotalByCategory();
                        break;
                    case "10":
                        ShowTotalByMonth();
                        break;
                    case "11":
                        ShowExpensesByPeriod();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        private void ListExpenses()
        {
            List<Expense> expenses = _expenseService.GetAll();

            if (expenses.Count == 0)
            {
                Console.WriteLine("Nenhuma despesa cadastrada.");
                return;
            }

            Console.WriteLine("\n--- Despesas ---");
            foreach (var expense in expenses)
            {
                string categoria = expense.Category?.Name ?? "Sem categoria";
                Console.WriteLine($"[{expense.Id}] {expense.Name} - R$ {expense.Amount} - {expense.Date:dd/MM/yyyy} - {categoria}");
            }
        }

        private void RegisterExpense()
        {
            Console.Write("Nome da despesa: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            Console.Write("Valor da despesa: ");
            string? amountInput = Console.ReadLine();

            if (!decimal.TryParse(amountInput, out decimal amount))
            {
                Console.WriteLine("Valor inválido.");
                return;
            }

            Console.Write("Data da despesa (dd/MM/yyyy): ");
            string? dateInput = Console.ReadLine();
            if (!DateTime.TryParse(dateInput, out DateTime date))
            {
                Console.WriteLine("Data inválida.");
                return;
            }

            var categorias = _categoryService.GetAll();
            foreach (var c in categorias)
            {
                Console.WriteLine($"[{c.Id}] {c.Name}");
            }

            Console.Write("Id da categoria: ");
            string? categoryIdInput = Console.ReadLine();
            if (!int.TryParse(categoryIdInput, out int categoryId))
            {
                Console.WriteLine("Id de categoria inválido.");
                return;
            }

            var expense = new Expense
            {
                Name = name,
                Amount = amount,
                Date = date,
                CategoryId = categoryId
            };

            try
            {
                _expenseService.Add(expense);
                Console.WriteLine("Despesa cadastrada com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        private void EditExpense()
        {
            ListExpenses();

            Console.Write("Selecione o Id da despesa que deseja editar: ");
            string? expenseIdInput = Console.ReadLine();
            if (!int.TryParse(expenseIdInput, out int expenseId))
            {
                Console.WriteLine("Id de despesa inválido.");
                return;
            }

            var existingExpense = _expenseService.GetById(expenseId);
            if (existingExpense == null)
            {
                Console.WriteLine("Despesa não encontrada.");
                return;
            }

            Console.Write("Nome da despesa: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            Console.Write("Valor da despesa: ");
            string? amountInput = Console.ReadLine();

            if (!decimal.TryParse(amountInput, out decimal amount))
            {
                Console.WriteLine("Valor inválido.");
                return;
            }

            Console.Write("Data da despesa (dd/MM/yyyy): ");
            string? dateInput = Console.ReadLine();
            if (!DateTime.TryParse(dateInput, out DateTime date))
            {
                Console.WriteLine("Data inválida.");
                return;
            }

            var categorias = _categoryService.GetAll();
            foreach (var c in categorias)
            {
                Console.WriteLine($"[{c.Id}] {c.Name}");
            }

            Console.Write("Id da categoria: ");
            string? categoryIdInput = Console.ReadLine();
            if (!int.TryParse(categoryIdInput, out int categoryId))
            {
                Console.WriteLine("Id de categoria inválido.");
                return;
            }

            var newExpense = new Expense
            {
                Id = expenseId,
                Name = name,
                Amount = amount,
                Date = date,
                CategoryId = categoryId
            };

            try
            {
                _expenseService.Update(newExpense);
                Console.WriteLine("Despesa editada com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        private void DeleteExpense()
        {
            ListExpenses();

            Console.Write("Selecione o Id da despesa que deseja excluir: ");
            string? expenseIdInput = Console.ReadLine();
            if (!int.TryParse(expenseIdInput, out int expenseId))
            {
                Console.WriteLine("Id de despesa inválido.");
                return;
            }

            try
            {
                _expenseService.Delete(expenseId);
                Console.WriteLine("Despesa excluída com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        private void ListCategory()
        {
            List<Category> categories = _categoryService.GetAll();

            if (categories.Count == 0)
            {
                Console.WriteLine("Nenhuma categoria cadastrada.");
                return;
            }

            Console.WriteLine("\n--- Categorias ---");
            foreach (var category in categories)
            {
                Console.WriteLine($"[{category.Id}] {category.Name}");
            }
        }

        private void RegisterCategory()
        {
            Console.Write("Nome da categoria: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            var category = new Category
            {
                Name = name
            };

            try
            {
                _categoryService.Add(category);
                Console.WriteLine("Categoria cadastrada com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        private void EditCategory()
        {
            ListCategory();

            Console.Write("Selecione o Id da categoria que deseja editar: ");
            string? categoryIdInput = Console.ReadLine();
            if (!int.TryParse(categoryIdInput, out int categoryId))
            {
                Console.WriteLine("Id da categoria inválido.");
                return;
            }

            var existingCategory = _categoryService.GetById(categoryId);
            if (existingCategory == null)
            {
                Console.WriteLine("Categoria não encontrada.");
                return;
            }

            Console.Write("Nome da categoria: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            var category = new Category
            {
                Id = categoryId,
                Name = name
            };

            try
            {
                _categoryService.Update(category);
                Console.WriteLine("Categoria editada com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        private void DeleteCategory()
        {
            ListCategory();

            Console.Write("Selecione o Id da categoria que deseja excluir: ");
            string? categoryIdInput = Console.ReadLine();
            if (!int.TryParse(categoryIdInput, out int categoryId))
            {
                Console.WriteLine("Id de categoria inválido.");
                return;
            }

            try
            {
                _categoryService.Delete(categoryId);
                Console.WriteLine("Categoria excluída com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        private void ShowTotalByCategory()
        {
            List<CategoryTotal> totalByCategories = _reportService.GetTotalByCategory();

            if (totalByCategories.Count == 0)
            {
                Console.WriteLine("Nenhum valor encontrado.");
                return;
            }

            Console.WriteLine("\n--- Valor total por categoria ---");
            foreach (var totalByCategorie in totalByCategories)
            {
                Console.WriteLine($"[{totalByCategorie.CategoryName}] {totalByCategorie.Total:C}");
            }
        }

        private void ShowTotalByMonth()
        {
            Console.Write("Digite o ano que deseja saber o total por mês (YYYY): ");

            string? yearInput = Console.ReadLine();
            if (!int.TryParse(yearInput, out int year))
            {
                Console.WriteLine("Data inválida.");
                return;
            }

            List<MonthlyTotal> totalByMonths = _reportService.GetTotalByMonth(year);

            if (totalByMonths.Count == 0)
            {
                Console.WriteLine("Nenhum valor encontrado.");
                return;
            }

            Console.WriteLine("\n--- Valor total por mês ---");
            foreach (var totalByMonth in totalByMonths)
            {
                Console.WriteLine($"[{totalByMonth.Month}] - {totalByMonth.Total:C}");
            }
        }

        private void ShowExpensesByPeriod()
        {
            Console.Write("Data do início do período (dd/MM/yyyy): ");
            string? startDateInput = Console.ReadLine();
            if (!DateTime.TryParse(startDateInput, out DateTime startDate))
            {
                Console.WriteLine("Data inválida.");
                return;
            }

            Console.Write("Data do fim do período (dd/MM/yyyy): ");
            string? endDateInput = Console.ReadLine();
            if (!DateTime.TryParse(endDateInput, out DateTime endDate))
            {
                Console.WriteLine("Data inválida.");
                return;
            }

            List<Expense> expensesByPeriod = _reportService.GetExpensesByPeriod(startDate, endDate);

            if (expensesByPeriod.Count == 0)
            {
                Console.WriteLine("Nenhuma despesa no período encontrada.");
                return;
            }

            Console.WriteLine("\n--- Despesas no período ---");
            foreach (var expense in expensesByPeriod)
            {
                string categoria = expense.Category?.Name ?? "Sem categoria";
                Console.WriteLine($"[{expense.Id}] {expense.Name} - R$ {expense.Amount} - {expense.Date:dd/MM/yyyy} - {categoria}");
            }
        }
    }
}
