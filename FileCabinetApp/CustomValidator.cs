using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    class CustomValidator : IRecordValidator
    {

        public void ValidateParameters(RecordParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.FirstName) ||
                  parameters.FirstName.Length < 2 ||
                  parameters.FirstName.Length > 60)
            {
                throw new ArgumentException("Invalid first name.");
            }

            // add check that name starts from upercase letter
            if (!char.IsUpper(parameters.FirstName[0]))
            {
                throw new ArgumentException("Name must start with uppercase letter");
            }

            if (string.IsNullOrWhiteSpace(parameters.LastName) ||
                parameters.LastName.Length < 2 ||
                parameters.LastName.Length > 60)
            {
                throw new ArgumentException("Invalid last name.");
            }

            // allow older age
            if (parameters.DateOfBirth < new DateTime(1900, 1, 1) ||
                parameters.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Invalid date of birth.");
            }
        }
    }
}
