// <copyright file="FileCabinetService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileCabinetApp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides core functionality for managing and manipulating file cabinet records.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">The validator used to validate record parameters.</param>
        public FileCabinetService(IRecordValidator validator) => this.validator = validator;

        /// <inheritdoc/>
        public int CreateRecord(RecordParameters parameters)
        {
            this.validator.ValidateParameters(parameters);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = parameters.DateOfBirth,
                Salary = parameters.Salary,
                Age = parameters.Age,
                Gender = parameters.Gender,
            };

            this.list.Add(record);

            var firstNameKey = parameters.FirstName.ToLower();
            if (!this.firstNameDictionary.ContainsKey(firstNameKey))
            {
                this.firstNameDictionary[firstNameKey] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[firstNameKey].Add(record);

            var lastNameKey = parameters.LastName.ToLower();
            if (!this.lastNameDictionary.ContainsKey(lastNameKey))
            {
                this.lastNameDictionary[lastNameKey] = new List<FileCabinetRecord>();
            }

            this.lastNameDictionary[lastNameKey].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(parameters.DateOfBirth))
            {
                this.dateOfBirthDictionary[parameters.DateOfBirth] = new List<FileCabinetRecord>();
            }

            this.dateOfBirthDictionary[parameters.DateOfBirth].Add(record);

            return record.Id;
        }

        /// <summary>
        /// Retrieves all records from the file cabinet.
        /// </summary>
        /// <returns>A read-only collection of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Gets the total number of records in the file cabinet.
        /// </summary>
        /// <returns>The count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Prints the provided records to the console.
        /// </summary>
        /// <param name="records">The collection of records to print.</param>
        public static void PrintRecords(ReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records.Count == 0)
            {
                Console.WriteLine("No records found.");
            }
            else
            {
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}");
                }
            }
        }

        /// <summary>
        /// Creates a snapshot of the current state of the file cabinet.
        /// </summary>
        /// <returns>A snapshot containing all current records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list);
        }

        /// <summary>
        /// Edits an existing record with new parameters.
        /// </summary>
        /// <param name="id">The ID of the record to edit.</param>
        /// <param name="parameters">The new parameters for the record.</param>
        /// <exception cref="ArgumentException">Thrown when the record with specified ID is not found.</exception>
        public void EditRecord(int id, RecordParameters parameters)
        {
            this.validator.ValidateParameters(parameters);

            var record = this.list.FirstOrDefault(r => r.Id == id);
            if (record == null)
            {
                throw new ArgumentException("Record not found.");
            }

            this.firstNameDictionary[record.FirstName.ToLower()].Remove(record);
            this.lastNameDictionary[record.LastName.ToLower()].Remove(record);
            this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);

            record.FirstName = parameters.FirstName;
            record.LastName = parameters.LastName;
            record.DateOfBirth = parameters.DateOfBirth;
            record.Salary = parameters.Salary;
            record.Age = parameters.Age;
            record.Gender = parameters.Gender;

            if (!this.firstNameDictionary.ContainsKey(parameters.FirstName.ToLower()))
            {
                this.firstNameDictionary[parameters.FirstName.ToLower()] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[parameters.FirstName.ToLower()].Add(record);

            if (!this.lastNameDictionary.ContainsKey(parameters.LastName.ToLower()))
            {
                this.lastNameDictionary[parameters.LastName.ToLower()] = new List<FileCabinetRecord>();
            }

            this.lastNameDictionary[parameters.LastName.ToLower()].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(parameters.DateOfBirth))
            {
                this.dateOfBirthDictionary[parameters.DateOfBirth] = new List<FileCabinetRecord>();
            }

            this.dateOfBirthDictionary[parameters.DateOfBirth].Add(record);
        }

        /// <summary>
        /// Finds records by first name.
        /// </summary>
        /// <param name="firstName">The first name to search for (case-insensitive).</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.firstNameDictionary.TryGetValue(firstName.ToLower(), out var records)
                ? new ReadOnlyCollection<FileCabinetRecord>(records)
                : new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Finds records by last name.
        /// </summary>
        /// <param name="lastName">The last name to search for (case-insensitive).</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.lastNameDictionary.TryGetValue(lastName.ToLower(), out var records)
                ? new ReadOnlyCollection<FileCabinetRecord>(records)
                : new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Finds records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            return this.dateOfBirthDictionary.TryGetValue(dateOfBirth, out var records)
                ? new ReadOnlyCollection<FileCabinetRecord>(records)
                : new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }
    }
}
