using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace ToDoItems.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? Completed { get; set; }
        public string CategoryName { get; set; }
    }

    public class ToDoItemManager
    {
        private string _connectionString;

        public ToDoItemManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Category> GetCategories()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories";
            connection.Open();
            var reader = cmd.ExecuteReader();
            List<Category> categories = new List<Category>();
            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"]
                });
            }
            return categories;
        }

        public Category GetCategoryById(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();
            reader.Read();
            return (new Category
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
            });
        }

        public void AddCategory(string name)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Categories VALUES (@name)";
            cmd.Parameters.AddWithValue("@name", name);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateCategory(Category c)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Categories SET Name = @name WHERE Id = @id";
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@Id", c.Id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void AddItem(string title, DateTime dueDate, int categoryId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Items VALUES (@title, @categoryId, @dueDate, @dateCompleted)";
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@categoryId", categoryId);
            cmd.Parameters.AddWithValue("@dueDate", dueDate);
            cmd.Parameters.AddWithValue("@dateCompleted", DBNull.Value);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Item> GetUncompletedItems()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT i.*, c.Name as CategoryName " +
                "FROM Items i " +
                "JOIN Categories c " +
                "ON i.CategoryId = c.Id " +
                "WHERE DateCompleted IS NULL";
            connection.Open();
            var reader = cmd.ExecuteReader();
            List<Item> items = new List<Item>();
            while (reader.Read())
            {
                items.Add(new Item
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    CategoryName = (string)reader["CategoryName"],
                    DueDate = (DateTime)reader["DueDate"],
                });
            }
            return items;
        }

        public List<Item> GetCompletedItems()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT i.*, c.Name as CategoryName " +
                "FROM Items i " +
                "JOIN Categories c " +
                "ON i.CategoryId = c.Id " +
                "WHERE DateCompleted IS NOT NULL";
            connection.Open();
            var reader = cmd.ExecuteReader();
            List<Item> items = new List<Item>();
            while (reader.Read())
            {
                items.Add(new Item
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    CategoryName = (string)reader["CategoryName"],
                    DueDate = (DateTime)reader["DueDate"],
                    Completed = (DateTime)reader["DateCompleted"],
                });
            }
            return items;
        }

        public void MarkAsCompleted(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Items SET DateCompleted = @date WHERE Id = @id";
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Item> GetItemsForCategory(int catId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT i.*, c.Name as CategoryName " +
                "FROM Items i " +
                "JOIN Categories c " +
                "ON i.CategoryId = c.Id " +
                "WHERE c.Id = @id";
            cmd.Parameters.AddWithValue("@id", catId);
            connection.Open();
            var reader = cmd.ExecuteReader();
            List<Item> items = new List<Item>();
            while (reader.Read())
            {
                items.Add(new Item
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    CategoryName = (string)reader["CategoryName"],
                    DueDate = (DateTime)reader["DueDate"],
                    Completed = reader.GetOrNull<DateTime?>("DateCompleted"),
                });
            }
            return items;
        }
    }
}
