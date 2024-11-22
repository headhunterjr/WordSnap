// <copyright file="IDatabaseConnection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapConsoleAppTests
{
    using Npgsql;

    /// <summary>
    /// Database connection interface.
    /// </summary>
    public interface IDatabaseConnection : IDisposable
    {
        /// <summary>
        /// Open connection.
        /// </summary>
        void Open();

        /// <summary>
        /// Create a command.
        /// </summary>
        /// <returns>command.</returns>
        NpgsqlCommand CreateCommand();

        /// <summary>
        /// Close a connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Execute reader.
        /// </summary>
        /// <param name="commandText">command text.</param>
        /// <returns>read value.</returns>
        IDataReaderWrapper ExecuteReader(string commandText);
    }
}
