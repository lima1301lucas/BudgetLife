using Microsoft.Extensions.Configuration;
using BudgetLife.Data;
using BudgetLife.Repositories;
using BudgetLife.Services;
using BudgetLife.UI;

var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();

string connectionString = config.GetConnectionString("Default")!;
var dbConnection = new DatabaseConnection(connectionString);

try
{
    using var testConnection = dbConnection.CreateConnection();
    testConnection.Open();
}
catch (Exception ex)
{
    Console.WriteLine($"Não foi possível conectar ao banco de dados: {ex.Message}");
    return;
}

var categoryRepository = new CategoryRepository(dbConnection);
var expenseRepository = new ExpenseRepository(dbConnection);
var reportRepository = new ReportRepository(dbConnection);

var categoryService = new CategoryService(categoryRepository, expenseRepository);
var expenseService = new ExpenseService(expenseRepository, categoryRepository);
var reportService = new ReportService(reportRepository);

var menu = new MenuPrincipal(expenseService, categoryService, reportService);
menu.Init();