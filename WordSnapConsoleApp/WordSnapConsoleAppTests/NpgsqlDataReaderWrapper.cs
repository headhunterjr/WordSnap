// <copyright file="NpgsqlDataReaderWrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapConsoleAppTests
{
    using Npgsql;

    /// <summary>
    /// Npgsql data reader wrapper.
    /// </summary>
    public class NpgsqlDataReaderWrapper : IDataReaderWrapper
    {
        private readonly NpgsqlDataReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpgsqlDataReaderWrapper"/> class.
        /// </summary>
        /// <param name="reader">reader.</param>
        public NpgsqlDataReaderWrapper(NpgsqlDataReader reader)
        {
            this.reader = reader;
        }

        /// <inheritdoc/>
        public int FieldCount => this.reader.FieldCount;

        /// <inheritdoc/>
        public bool HasRows => this.reader.HasRows;

        /// <inheritdoc/>
        public string GetName(int i) => this.reader.GetName(i);

        /// <inheritdoc/>
        public object GetValue(int i) => this.reader.GetValue(i);

        /// <inheritdoc/>
        public bool Read() => this.reader.Read();

        /// <inheritdoc/>
        public void Dispose()
        {
            this.reader.Dispose();
        }
    }
}
