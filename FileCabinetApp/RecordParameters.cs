// <copyright file="RecordParameters.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileCabinetApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record RecordParameters(
      string FirstName,
      string LastName,
      DateTime DateOfBirth,
      short Age,
      decimal Salary,
      char Gender);
}
