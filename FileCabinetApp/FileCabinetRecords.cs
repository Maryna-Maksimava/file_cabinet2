using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents a record in the File Cabinet application.
    /// </summary>
    public class FileCabinetRecords
    {
        /// <summary>
        /// Gets or sets the unique identifier for the record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the person.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the person.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the person.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the age of the person.
        /// </summary>
        public short Age { get; set; }

        /// <summary>
        /// Gets or sets the salary of the person.
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets the gender of the person.
        /// </summary>
        public char Gender { get; set; }
    }
}
