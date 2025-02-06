using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
	public class FileCabinetService
	{
		private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
		private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

		private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

		public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
		{
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
                throw new ArgumentException("Invalid first name.");

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
                throw new ArgumentException("Invalid last name.");

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
				throw new ArgumentException("Invalid date of birth.");

			var record = new FileCabinetRecord
			{
				Id = this.list.Count + 1,
				FirstName = firstName,
				LastName = lastName,
				DateOfBirth = dateOfBirth,
				Salary = salary,
				Age = age,
				Gender = gender,
			};

			this.list.Add(record);

			if (!this.firstNameDictionary.ContainsKey(firstName.ToLower()))
			{
				this.firstNameDictionary[firstName.ToLower()] = new List<FileCabinetRecord>();
			}
			this.firstNameDictionary[firstName.ToLower()].Add(record);

			if (!this.lastNameDictionary.ContainsKey(lastName.ToLower()))
			{
				this.lastNameDictionary[lastName.ToLower()] = new List<FileCabinetRecord>();
			}
			this.lastNameDictionary[lastName.ToLower()].Add(record);

			if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
			{
				this.dateOfBirthDictionary[dateOfBirth] = new List<FileCabinetRecord>();
			}
			this.dateOfBirthDictionary[dateOfBirth].Add(record);

			return record.Id;
		}

		public FileCabinetRecord[] GetRecords()
		{
			return this.list.ToArray();
		}

		public int GetStat()
		{
			return this.list.Count;
		}

		public static void PrintRecords(FileCabinetRecord[] records)
		{
			if (records.Length == 0)
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


		public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
		{
			var record = this.list.FirstOrDefault(r => r.Id == id);
			if (record == null)
				throw new ArgumentException("Record not found.");

			this.firstNameDictionary[record.FirstName.ToLower()].Remove(record);
			this.lastNameDictionary[record.LastName.ToLower()].Remove(record);
			this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);

			record.FirstName = firstName;
			record.LastName = lastName;
			record.DateOfBirth = dateOfBirth;
			record.Salary = salary;
			record.Age = age;
			record.Gender = gender;

			if (!this.firstNameDictionary.ContainsKey(firstName.ToLower()))
				this.firstNameDictionary[firstName.ToLower()] = new List<FileCabinetRecord>();
			this.firstNameDictionary[firstName.ToLower()].Add(record);

			if (!this.lastNameDictionary.ContainsKey(lastName.ToLower()))
				this.lastNameDictionary[lastName.ToLower()] = new List<FileCabinetRecord>();
			this.lastNameDictionary[lastName.ToLower()].Add(record);

			if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
				this.dateOfBirthDictionary[dateOfBirth] = new List<FileCabinetRecord>();
			this.dateOfBirthDictionary[dateOfBirth].Add(record);
		}


		public FileCabinetRecord[] FindByFirstName(string firstName)
		{
			return this.firstNameDictionary.TryGetValue(firstName.ToLower(), out var records)
				? records.ToArray()
				: Array.Empty<FileCabinetRecord>();
		}

		public FileCabinetRecord[] FindByLastName(string lastName)
		{
			return this.lastNameDictionary.TryGetValue(lastName.ToLower(), out var records)
				? records.ToArray()
				: Array.Empty<FileCabinetRecord>();
		}

		public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
		{
			return this.dateOfBirthDictionary.TryGetValue(dateOfBirth, out var records)
				? records.ToArray()
				: Array.Empty<FileCabinetRecord>();
		}
	}
}
