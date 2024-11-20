using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSnapConsoleAppTests
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly NpgsqlConnection _connection;

        public DatabaseConnection(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        public void Open() => _connection.Open();

        public NpgsqlCommand CreateCommand() => _connection.CreateCommand();

        public void Close() => _connection.Close();

        public void Dispose() => _connection.Dispose();

        public IDataReaderWrapper ExecuteReader(string commandText)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            var reader = command.ExecuteReader();
            return new NpgsqlDataReaderWrapper(reader);
        }
    }
}
