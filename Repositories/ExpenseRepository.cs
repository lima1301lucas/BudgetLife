using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetLife.Data;
using BudgetLife.Models;
using MySqlConnector;

namespace BudgetLife.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private const string SelectWithCategoryJoin = @"
            SELECT e.id, e.name, e.amount, e.date, e.category_id, c.name AS category_name 
            FROM expenses e
            JOIN categories c ON e.category_id = c.id";

        private readonly DatabaseConnection _databaseConnection;

        public ExpenseRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public Expense? GetById(int id)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand($"{SelectWithCategoryJoin} WHERE e.id = @id;", connection);
            commmand.Parameters.AddWithValue("@id", id);
            using var reader = commmand.ExecuteReader();

            if (reader.Read())
            {
                return MapReaderToExpenseWithCategory(reader);
            }
            return null;
        }

        public List<Expense> GetAll() 
        {
            var expenses = new List<Expense>();
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand(SelectWithCategoryJoin, connection);
            using var reader = commmand.ExecuteReader();

            while (reader.Read())
            {
                expenses.Add(MapReaderToExpenseWithCategory(reader));
            }
            return expenses;
        }

        public void Add(Expense expense)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand("INSERT INTO expenses (name, amount, date, category_id) VALUES (@name, @amount, @date, @category_id);", connection);
            commmand.Parameters.AddWithValue("@name", expense.Name);
            commmand.Parameters.AddWithValue("@amount", expense.Amount);
            commmand.Parameters.AddWithValue("@date", expense.Date);
            commmand.Parameters.AddWithValue("@category_id", expense.CategoryId);
            commmand.ExecuteNonQuery();
        }

        public void Update(Expense expense) 
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand("UPDATE expenses SET name = @name, amount = @amount, date = @date, category_id = @category_id WHERE id = @id;", connection);
            commmand.Parameters.AddWithValue("@name", expense.Name);
            commmand.Parameters.AddWithValue("@amount", expense.Amount);
            commmand.Parameters.AddWithValue("@date", expense.Date);
            commmand.Parameters.AddWithValue("@category_id", expense.CategoryId);
            commmand.Parameters.AddWithValue("@id", expense.Id);
            commmand.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand("DELETE FROM expenses WHERE id = @id;", connection);
            commmand.Parameters.AddWithValue("@id", id);
            commmand.ExecuteNonQuery();
        }

        private static Expense MapReaderToExpenseWithCategory(MySqlDataReader reader)
        {
            return new Expense
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
            };
        }
    }
}
