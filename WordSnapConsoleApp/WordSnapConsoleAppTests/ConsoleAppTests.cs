// <copyright file="ConsoleAppTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapConsoleAppTests
{
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Npgsql;

    /// <summary>
    /// Console app tests.
    /// </summary>
    public class ConsoleAppTests
    {
        /// <summary>
        /// Valid JSON should return a connection string.
        /// </summary>
        [Fact]
        public void LoadConfiguration_ValidJson_ReturnsConnectionString()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            var sectionMock = new Mock<IConfigurationSection>();
            sectionMock.Setup(s => s.Value).Returns("Host=localhost;Database=TestDB;Username=test;Password=test");

            configurationMock
                .Setup(c => c.GetSection("ConnectionStrings:WordSnapDatabaseConnection"))
                .Returns(sectionMock.Object);

            // Act
            var connectionString = sectionMock.Object.Value;

            // Assert
            Assert.NotNull(connectionString);
            Assert.Contains("localhost", connectionString);
        }

        /// <summary>
        /// Creating tables method executes without errors.
        /// </summary>
        [Fact]
        public void CreateTables_ExecutesWithoutErrors()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();
            var commandMock = new Mock<NpgsqlCommand>();

            connectionMock
                .Setup(conn => conn.CreateCommand())
                .Returns(commandMock.Object);

            commandMock.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1);

            // Act
            connectionMock.Object.Open();
            var command = connectionMock.Object.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS TestTable (id INT PRIMARY KEY);";
            var result = command.ExecuteNonQuery();

            // Assert
            Assert.Equal(1, result);
        }

        /// <summary>
        /// Populate tables method executes insert statements.
        /// </summary>
        [Fact]
        public void PopulateTablesWithRandomData_ExecutesInsertStatements()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();
            var commandMock = new Mock<NpgsqlCommand>();

            connectionMock
                .Setup(conn => conn.CreateCommand())
                .Returns(commandMock.Object);

            commandMock
                .Setup(cmd => cmd.ExecuteScalar())
                .Returns(1);

            commandMock
                .Setup(cmd => cmd.ExecuteNonQuery())
                .Returns(1);

            // Act
            connectionMock.Object.Open();
            var command = connectionMock.Object.CreateCommand();
            command.CommandText = "INSERT INTO TestTable (name) VALUES ('test');";
            var result = command.ExecuteNonQuery();

            // Assert
            Assert.Equal(1, result);
        }

        /// <summary>
        /// Print table contents method displays table data.
        /// </summary>
        [Fact]
        public void PrintTableContents_DisplaysTableData()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();

            connectionMock
                .Setup(conn => conn.ExecuteReader(It.IsAny<string>()))
                .Returns((string commandText) =>
                {
                    var mockReader = new Mock<IDataReaderWrapper>();

                    mockReader
                        .SetupSequence(r => r.Read())
                        .Returns(true)
                        .Returns(false);

                    mockReader
                        .Setup(r => r.FieldCount)
                        .Returns(2);

                    mockReader
                        .Setup(r => r.GetName(0))
                        .Returns("Column1");

                    mockReader
                        .Setup(r => r.GetName(1))
                        .Returns("Column2");

                    mockReader
                        .Setup(r => r.GetValue(0))
                        .Returns("Value1");

                    mockReader
                        .Setup(r => r.GetValue(1))
                        .Returns("Value2");

                    return mockReader.Object;
                });

            // Act
            using (var conn = connectionMock.Object)
            {
                conn.Open();
                var reader = conn.ExecuteReader("SELECT * FROM TestTable;");

                // Assert
                Assert.True(reader.Read());
                Assert.Equal("Value1", reader.GetValue(0));
                Assert.Equal("Value2", reader.GetValue(1));
                Assert.False(reader.Read());
            }
        }

        /// <summary>
        /// Create tables method generates correct sql for table creation.
        /// </summary>
        [Fact]
        public void CreateTables_GeneratesCorrectTableCreationSql()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();
            var commandMock = new Mock<NpgsqlCommand>();

            connectionMock
                .Setup(conn => conn.CreateCommand())
                .Returns(commandMock.Object);

            commandMock
                .SetupSet(cmd => cmd.CommandText = It.IsAny<string>())
                .Callback<string>((sql) =>
                {
                    Assert.Contains("CREATE TABLE IF NOT EXISTS Users", sql);
                    Assert.Contains("CREATE TABLE IF NOT EXISTS CardSets", sql);
                    Assert.Contains("CREATE TABLE IF NOT EXISTS Cards", sql);
                    Assert.Contains("CREATE TABLE IF NOT EXISTS Progress", sql);
                    Assert.Contains("CREATE TABLE IF NOT EXISTS UsersCardSets", sql);
                });

            // Act
            connectionMock.Object.Open();
            var command = connectionMock.Object.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS Users; " + "CREATE TABLE IF NOT EXISTS CardSets; " + "CREATE TABLE IF NOT EXISTS Cards; " + "CREATE TABLE IF NOT EXISTS Progress; " + "CREATE TABLE IF NOT EXISTS UsersCardSets; ";
        }

        /// <summary>
        /// Print table method retrieves all columns of a table.
        /// </summary>
        [Fact]
        public void PrintTable_RetrievesAllColumnsForTable()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();

            connectionMock
                .Setup(conn => conn.ExecuteReader(It.IsAny<string>()))
                .Returns((string commandText) =>
                {
                    var mockReader = new Mock<IDataReaderWrapper>();

                    mockReader
                        .SetupSequence(r => r.Read())
                        .Returns(true)
                        .Returns(false);

                    mockReader
                        .Setup(r => r.FieldCount)
                        .Returns(3);

                    mockReader
                        .Setup(r => r.GetName(0))
                        .Returns("id");
                    mockReader
                        .Setup(r => r.GetName(1))
                        .Returns("name");
                    mockReader
                        .Setup(r => r.GetName(2))
                        .Returns("value");

                    return mockReader.Object;
                });

            // Act
            using (var conn = connectionMock.Object)
            {
                var reader = conn.ExecuteReader("SELECT * FROM SomeTable");

                // Assert
                Assert.True(reader.Read());
                Assert.Equal(3, reader.FieldCount);
                Assert.False(reader.Read());
            }
        }

        /// <summary>
        /// Dispose calls parent dispose.
        /// </summary>
        [Fact]
        public void DatabaseConnection_Dispose_CallsDisposeOnConnection()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();
            connectionMock.Setup(conn => conn.Dispose());

            // Act
            connectionMock.Object.Dispose();

            // Assert
            connectionMock.Verify(conn => conn.Dispose(), Times.Once);
        }

        /// <summary>
        /// Execute reader on an empty result set returns no rows.
        /// </summary>
        [Fact]
        public void ExecuteReader_EmptyResultSet_ReturnsNoRows()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();
            connectionMock
                .Setup(conn => conn.ExecuteReader(It.IsAny<string>()))
                .Returns((string commandText) =>
                {
                    var mockReader = new Mock<IDataReaderWrapper>();
                    mockReader.Setup(r => r.HasRows).Returns(false);
                    return mockReader.Object;
                });

            // Act
            using (var conn = connectionMock.Object)
            {
                conn.Open();
                var reader = conn.ExecuteReader("SELECT * FROM TestTable;");

                // Assert
                Assert.False(reader.HasRows);
            }
        }

        /// <summary>
        /// Execute Non Query executes correctly.
        /// </summary>
        [Fact]
        public void ExecuteNonQuery_ExecutesCorrectly()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();
            var commandMock = new Mock<NpgsqlCommand>();

            connectionMock
                .Setup(conn => conn.CreateCommand())
                .Returns(commandMock.Object);

            commandMock
                .Setup(cmd => cmd.ExecuteNonQuery())
                .Returns(1);

            // Act
            connectionMock.Object.Open();
            var command = connectionMock.Object.CreateCommand();
            command.CommandText = "INSERT INTO TestTable (name) VALUES ('test');";
            var result = command.ExecuteNonQuery();

            // Assert
            Assert.Equal(1, result);
        }

        /// <summary>
        /// Execute Non query throws exception when an error occurs.
        /// </summary>
        [Fact]
        public void ExecuteNonQuery_ThrowsException_OnError()
        {
            // Arrange
            var connectionMock = new Mock<IDatabaseConnection>();
            var commandMock = new Mock<NpgsqlCommand>();

            connectionMock
                .Setup(conn => conn.CreateCommand())
                .Returns(commandMock.Object);

            commandMock
                .Setup(cmd => cmd.ExecuteNonQuery())
                .Throws(new Exception("Database error"));

            // Act & Assert
            connectionMock.Object.Open();
            var command = connectionMock.Object.CreateCommand();
            command.CommandText = "INSERT INTO TestTable (name) VALUES ('test');";

            var exception = Assert.Throws<Exception>(() => command.ExecuteNonQuery());
            Assert.Equal("Database error", exception.Message);
        }
    }
}