using BudgetLife.Data;
using BudgetLife.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLife.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DatabaseConnection _databaseConnection;

        public ReportRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public List<CategoryTotal> GetTotalByCategory()
        {
            var results = new List<CategoryTotal>();

            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            const string sql = @"
                SELECT c.name AS category_name, SUM(e.amount) AS total
                FROM expenses e
                JOIN categories c ON e.category_id = c.id
                GROUP BY c.id, c.name";

            using var command = new MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new CategoryTotal
                {
                    CategoryName = reader.GetString("category_name"),
                    Total = reader.GetDecimal("total")
                });
            }

            return results;
        }

        public List<MonthlyTotal> GetTotalByMonth(int year)
        {
            var results = new List<MonthlyTotal>();

            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            const string sql = @"
                SELECT MONTH(date) AS month, SUM(amount) AS total
                FROM expenses
                WHERE YEAR(date) = @year
                GROUP BY MONTH(date)";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@year", year);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new MonthlyTotal
                {
                    Month = reader.GetInt32("month"),
                    Total = reader.GetDecimal("total")
                });
            }

            return results;
        }

        public List<Expense> GetExpensesByPeriod(DateTime start, DateTime end)
        {
            var results = new List<Expense>();

            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            const string sql = @"
                SELECT e.id, e.name, e.amount, e.date, e.category_id, c.name AS category_name
                FROM expenses e
                JOIN categories c ON e.category_id = c.id
                WHERE e.date BETWEEN @start AND @end";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@start", start);
            command.Parameters.AddWithValue("@end", end);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new Expense
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Amount = reader.GetDecimal("amount"),
                    Date = reader.GetDateTime("date"),
                    CategoryId = reader.GetInt32("category_id"),
                    Category = new Category
                    {
                        Id = reader.GetInt32("category_id"),
                        Name = reader.GetString("category_name")
                    }
                });
            }

            return results;
        }
    }
}
