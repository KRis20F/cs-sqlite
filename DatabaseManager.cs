using System.Data.SQLite;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath};Version=3;";
        CreateDatabase();
    }

    private void CreateDatabase()
    {
        if (!File.Exists("products.db"))
        {
            SQLiteConnection.CreateFile("products.db");
        }

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string sql = @"
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Price DECIMAL(10,2) NOT NULL,
                Quantity INTEGER NOT NULL
            )";

        using var command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
    }

    public void InsertProduct(Product product)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string sql = @"INSERT INTO Products (Name, Price, Quantity) VALUES (@name, @price, @quantity)";
        using var command = new SQLiteCommand(sql, connection);

        command.Parameters.AddWithValue("@name", product.Name);
        command.Parameters.AddWithValue("@price", product.Price);
        command.Parameters.AddWithValue("@quantity", product.Quantity);

        command.ExecuteNonQuery();
    }

    public List<Product> GetAllProducts()
    {
        var products = new List<Product>();
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string sql = "SELECT * FROM Products";
        using var command = new SQLiteCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                Quantity = reader.GetInt32(3)
            });
        }

        return products;
    }

    public List<Product> SearchProductsByName(string name)
    {
        var products = new List<Product>();
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string sql = "SELECT * FROM Products WHERE Name LIKE @name";
        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@name", $"%{name}%");

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                Quantity = reader.GetInt32(3)
            });
        }

        return products;
    }

    public void UpdateProduct(Product product)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string sql = @"UPDATE Products 
                      SET Name = @name, Price = @price, Quantity = @quantity 
                      WHERE Id = @id";

        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", product.Id);
        command.Parameters.AddWithValue("@name", product.Name);
        command.Parameters.AddWithValue("@price", product.Price);
        command.Parameters.AddWithValue("@quantity", product.Quantity);

        command.ExecuteNonQuery();
    }

    public void DeleteProduct(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string sql = "DELETE FROM Products WHERE Id = @id";
        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }
}