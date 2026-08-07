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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseConnection _databaseConnection;

        public CategoryRepository(DatabaseConnection databaseConnection)
        {
           _databaseConnection = databaseConnection;
        }

        public Category? GetById(int id) 
        { 
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand("SELECT id, name FROM categories WHERE id = @id;", connection);
            commmand.Parameters.AddWithValue("@id", id);
            using var reader = commmand.ExecuteReader();

            if (reader.Read())
            {
                return MapReaderToCategory(reader);
            }
            return null;
        }

        public List<Category> GetAll() 
        {
            var categories = new List<Category>();
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand("SELECT id, name FROM categories;", connection);
            using var reader = commmand.ExecuteReader();

            while (reader.Read()) 
            {
                categories.Add(MapReaderToCategory(reader));
            }
            return categories;
        }

        public void Add(Category category)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand("INSERT INTO categories (name) VALUES (@name);", connection);
            commmand.Parameters.AddWithValue("@name", category.Name);
            commmand.ExecuteNonQuery();
        }

        public void Update(Category category) 
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand("UPDATE categories SET name = @name WHERE id = @id;", connection);
            commmand.Parameters.AddWithValue("@name", category.Name);
            commmand.Parameters.AddWithValue("@id", category.Id);
            commmand.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var commmand = new MySqlCommand("DELETE FROM categories WHERE id = @id;", connection);
            commmand.Parameters.AddWithValue("@id", id);
            commmand.ExecuteNonQuery();
        }

        private static Category MapReaderToCategory(MySqlDataReader reader)
        {
            return new Category
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name")
            };
        }
    }
}
