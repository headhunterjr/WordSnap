// <copyright file="IDataReaderWrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapConsoleAppTests
{
    /// <summary>
    /// Data reader wrapper interface.
    /// </summary>
    public interface IDataReaderWrapper : IDisposable
    {
        /// <summary>
        /// Gets field count.
        /// </summary>
        int FieldCount { get; }

        /// <summary>
        /// Gets a value indicating whether it has rows.
        /// </summary>
        bool HasRows { get; }

        /// <summary>
        /// Gets name.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>name.</returns>
        string GetName(int i);

        /// <summary>
        /// Gets value.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>value.</returns>
        object GetValue(int i);

        /// <summary>
        /// Reads.
        /// </summary>
        /// <returns>value.</returns>
        bool Read();
    }
}
