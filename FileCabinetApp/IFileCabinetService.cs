// <copyright file="IFileCabinetService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileCabinetApp
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Defines core operations for managing file cabinet records.
    /// </summary>
    internal interface IFileCabinetService
    {
        /// <summary>
        /// Prints a collection of file cabinet records to the console.
        /// </summary>
        /// <param name="records">The collection of records to print.</param>
        static abstract void PrintRecords(ReadOnlyCollection<FileCabinetRecord> records);

        /// <summary>
        /// Creates a new record with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters for the new record.</param>
        /// <returns>The ID of the newly created record.</returns>
        int CreateRecord(RecordParameters parameters);

        /// <summary>
        /// Edits an existing record with new parameters.
        /// </summary>
        /// <param name="id">The ID of the record to edit.</param>
        /// <param name="parameters">The new parameters for the record.</param>
        void EditRecord(int id, RecordParameters parameters);

        /// <summary>
        /// Finds records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Finds records by first name.
        /// </summary>
        /// <param name="firstName">The first name to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds records by last name.
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Retrieves all records from the file cabinet.
        /// </summary>
        /// <returns>A read-only collection of all records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Creates a snapshot of the current state of the file cabinet.
        /// </summary>
        /// <returns>A snapshot containing all current records.</returns>
        FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Gets the total number of records in the file cabinet.
        /// </summary>
        /// <returns>The count of records.</returns>
        int GetStat();
    }
}