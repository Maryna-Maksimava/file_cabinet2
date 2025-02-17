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

        public int CreateRecord(RecordParameters parameters)
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


        public void EditRecord(int id, RecordParameters parameters)
        {
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
                this.firstNameDictionary[parameters.FirstName.ToLower()] = new List<FileCabinetRecord>();
            this.firstNameDictionary[parameters.FirstName.ToLower()].Add(record);

            if (!this.lastNameDictionary.ContainsKey(parameters.LastName.ToLower()))
                this.lastNameDictionary[parameters.LastName.ToLower()] = new List<FileCabinetRecord>();
            this.lastNameDictionary[parameters.LastName.ToLower()].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(parameters.DateOfBirth))
                this.dateOfBirthDictionary[parameters.DateOfBirth] = new List<FileCabinetRecord>();
            this.dateOfBirthDictionary[parameters.DateOfBirth].Add(record);
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
