using Microsoft.Extensions.Configuration;
using Npgsql;

class Program
{
    private static string? connectionString;

    static void Main(string[] args)
    {
        LoadConfiguration();
        CreateTables();
        PopulateTablesWithRandomData();
        PrintTableContents();
    }
    private static void LoadConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        connectionString = configuration.GetConnectionString("WordSnapDatabaseConnection");
    }

    private static void CreateTables()
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string createTablesSql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    user_id SERIAL PRIMARY KEY,
                    username VARCHAR(50) NOT NULL,
                    email VARCHAR(100) NOT NULL,
                    password_hash VARCHAR(255) NOT NULL,
                    is_verified BOOLEAN DEFAULT FALSE,
                    created_at TIMESTAMP DEFAULT NOW()
                );
                
                CREATE TABLE IF NOT EXISTS CardSets (
                    set_id SERIAL PRIMARY KEY,
                    user_id INT NOT NULL,
                    set_name VARCHAR(100) NOT NULL,
                    is_public BOOLEAN DEFAULT FALSE,
                    created_at TIMESTAMP DEFAULT NOW(),
                    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS Cards (
                    card_id SERIAL PRIMARY KEY,
                    set_id INT NOT NULL,
                    word_en VARCHAR(100) NOT NULL,
                    word_ua VARCHAR(100) NOT NULL,
                    comment TEXT,
                    FOREIGN KEY (set_id) REFERENCES CardSets(set_id) ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS Progress (
                    progress_id SERIAL PRIMARY KEY,
                    user_id INT NOT NULL,
                    set_id INT NOT NULL,
                    last_accessed TIMESTAMP DEFAULT NOW(),
                    success_rate FLOAT DEFAULT 0.0,
                    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE,
                    FOREIGN KEY (set_id) REFERENCES CardSets(set_id) ON DELETE CASCADE
                );";

            using (var cmd = new NpgsqlCommand(createTablesSql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
        Console.WriteLine("Tables created (if they didn't already exist).");
    }
    private static void PopulateTablesWithRandomData()
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            var random = new Random();

            for (int i = 1; i <= 25; i++)
            {
                string username = $"user_{random.Next(1, 1000)}";
                string email = $"{username}@example.com";
                string passwordHash = $"hash_{random.Next(10000, 99999)}";
                bool isVerified = random.Next(0, 2) == 1;
                DateTime createdAt = DateTime.Now.AddDays(-random.Next(1, 10));

                string insertUserSql = "INSERT INTO Users (username, email, password_hash, is_verified, created_at) VALUES (@username, @email, @passwordHash, @isVerified, @createdAt)";
                using (var cmd = new NpgsqlCommand(insertUserSql, conn))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("passwordHash", passwordHash);
                    cmd.Parameters.AddWithValue("isVerified", isVerified);
                    cmd.Parameters.AddWithValue("createdAt", createdAt);
                    cmd.ExecuteNonQuery();
                }
            }

            for (int i = 1; i <= 25; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string setName = $"Set_{random.Next(1, 100)}";
                    bool isPublic = random.Next(0, 2) == 1;
                    DateTime createdAt = DateTime.Now.AddDays(-random.Next(1, 100));

                    string insertCardSetSql = "INSERT INTO CardSets (user_id, set_name, is_public, created_at) VALUES (@userId, @setName, @isPublic, @createdAt)";
                    using (var cmd = new NpgsqlCommand(insertCardSetSql, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", i);
                        cmd.Parameters.AddWithValue("setName", setName);
                        cmd.Parameters.AddWithValue("isPublic", isPublic);
                        cmd.Parameters.AddWithValue("createdAt", createdAt);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            for (int i = 1; i <= 50; i++)
            {
                string wordEn = $"Word_EN_{random.Next(1, 100)}";
                string wordUa = $"Word_UA_{random.Next(1, 100)}";
                string? comment = random.Next(0, 2) == 1 ? $"Comment_{random.Next(1, 100)}" : null;

                string insertCardSql = "INSERT INTO Cards (set_id, word_en, word_ua, comment) VALUES (@setId, @wordEn, @wordUa, @comment)";
                using (var cmd = new NpgsqlCommand(insertCardSql, conn))
                {
                    cmd.Parameters.AddWithValue("setId", i);
                    cmd.Parameters.AddWithValue("wordEn", wordEn);
                    cmd.Parameters.AddWithValue("wordUa", wordUa);
                    if (comment != null)
                        cmd.Parameters.AddWithValue("comment", comment);
                    else
                        cmd.Parameters.AddWithValue("comment", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }

            for (int i = 1; i <= 25; i++)
            {
                for (int j = (i - 1) * 2 + 1; j <= i * 2; j++)
                {
                    float successRate = (float)random.NextDouble() * 100;
                    DateTime lastAccessed = DateTime.Now.AddDays(-random.Next(1, 100));

                    string insertProgressSql = "INSERT INTO Progress (user_id, set_id, last_accessed, success_rate) VALUES (@userId, @setId, @lastAccessed, @successRate)";
                    using (var cmd = new NpgsqlCommand(insertProgressSql, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", i);
                        cmd.Parameters.AddWithValue("setId", j);
                        cmd.Parameters.AddWithValue("lastAccessed", lastAccessed);
                        cmd.Parameters.AddWithValue("successRate", successRate);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        Console.WriteLine("Random data inserted into tables.");
    }
    private static void PrintTableContents()
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            PrintTable(conn, "Users", "user_id, username, email, password_hash, is_verified, created_at");
            PrintTable(conn, "CardSets", "set_id, user_id, set_name, is_public, created_at");
            PrintTable(conn, "Cards", "card_id, set_id, word_en, word_ua, comment");
            PrintTable(conn, "Progress", "progress_id, user_id, set_id, last_accessed, success_rate");
        }
    }
    private static void PrintTable(NpgsqlConnection conn, string tableName, string columns)
    {
        string selectSql = $"SELECT {columns} FROM {tableName}";
        using (var cmd = new NpgsqlCommand(selectSql, conn))
        {
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine($"\nTable: {tableName}");
                Console.WriteLine(new string('=', 50));
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader.GetValue(i)}\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
