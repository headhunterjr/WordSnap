// <copyright file="DatabaseConnection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapConsoleAppTests
{
    using Npgsql;

    /// <summary>
    /// Database connection class.
    /// </summary>
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly NpgsqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnection"/> class.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public DatabaseConnection(string connectionString)
        {
            this.connection = new NpgsqlConnection(connectionString);
        }

        /// <inheritdoc/>
        public void Open() => this.connection.Open();

        /// <inheritdoc/>
        public NpgsqlCommand CreateCommand() => this.connection.CreateCommand();

        /// <inheritdoc/>
        public void Close() => this.connection.Close();

        /// <inheritdoc/>
        public void Dispose() => this.connection.Dispose();

        /// <inheritdoc/>
        public IDataReaderWrapper ExecuteReader(string commandText)
        {
            var command = this.CreateCommand();
            command.CommandText = commandText;
            var reader = command.ExecuteReader();
            return new NpgsqlDataReaderWrapper(reader);
        }
    }
}
