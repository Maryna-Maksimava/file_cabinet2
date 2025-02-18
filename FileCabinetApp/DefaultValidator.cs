// <copyright file="DefaultValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileCabinetApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class DefaultValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.FirstName) ||
                  parameters.FirstName.Length < 2 ||
                  parameters.FirstName.Length > 60)
            {
                throw new ArgumentException("Invalid first name.");
            }

            if (string.IsNullOrWhiteSpace(parameters.LastName) ||
                parameters.LastName.Length < 2 ||
                parameters.LastName.Length > 60)
            {
                throw new ArgumentException("Invalid last name.");
            }

            if (parameters.DateOfBirth < new DateTime(1950, 1, 1) ||
                parameters.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Invalid date of birth.");
            }
        }
    }
}
