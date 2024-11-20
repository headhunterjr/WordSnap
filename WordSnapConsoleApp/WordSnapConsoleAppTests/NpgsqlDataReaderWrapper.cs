using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSnapConsoleAppTests
{
    public class NpgsqlDataReaderWrapper : IDataReaderWrapper
    {
        private readonly NpgsqlDataReader _reader;

        public NpgsqlDataReaderWrapper(NpgsqlDataReader reader)
        {
            _reader = reader;
        }

        public bool Read() => _reader.Read();
        public int FieldCount => _reader.FieldCount;
        public string GetName(int i) => _reader.GetName(i);
        public object GetValue(int i) => _reader.GetValue(i);
        public bool HasRows => _reader.HasRows;

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
