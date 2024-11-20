using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSnapConsoleAppTests
{
    public interface IDatabaseConnection : IDisposable
    {
        void Open();
        NpgsqlCommand CreateCommand();
        void Close();
        IDataReaderWrapper ExecuteReader(string commandText);
    }
}
