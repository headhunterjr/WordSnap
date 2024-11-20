using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSnapConsoleAppTests
{
    public interface IDataReaderWrapper : IDisposable
    {
        bool Read();
        int FieldCount { get; }
        string GetName(int i);
        object GetValue(int i);
        bool HasRows { get; }
    }
}
