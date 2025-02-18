// <copyright file="FileCabinetRecordCsvWriter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileCabinetApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides functionality to write FileCabinetRecord objects to CSV format.
    /// </summary>
    internal class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">The text writer to which the CSV content will be written.</param>
        /// <exception cref="ArgumentNullException">Thrown when writer is null.</exception>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Writes a single record to the CSV file.
        /// </summary>
        /// <param name="record">The record to write in CSV format.</param>
        /// <remarks>
        /// The record is written in the following format:
        /// Id,FirstName,LastName,DateOfBirth,Age,Salary,Gender
        /// </remarks>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine(
                $"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth:MM/dd/yyyy},{record.Age},{record.Salary},{record.Gender}");
        }

        /// <summary>
        /// Closes the CSV writer.
        /// </summary>
        public void Close()
        {
        }
    }
}