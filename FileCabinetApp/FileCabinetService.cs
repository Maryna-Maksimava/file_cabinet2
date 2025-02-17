using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace FileCabinetApp
{
    public class FileCabinetService : IFileCabinetService
    {
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private IRecordValidator validator;

        public FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }


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

        // Modify all methods that return arrays
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        public int GetStat()
        {
            return this.list.Count;
        }

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

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list);
        }


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
                this.firstNameDictionary[parameters.FirstName.ToLower()] = new List<FileCabinetRecord>();
            this.firstNameDictionary[parameters.FirstName.ToLower()].Add(record);

            if (!this.lastNameDictionary.ContainsKey(parameters.LastName.ToLower()))
                this.lastNameDictionary[parameters.LastName.ToLower()] = new List<FileCabinetRecord>();
            this.lastNameDictionary[parameters.LastName.ToLower()].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(parameters.DateOfBirth))
                this.dateOfBirthDictionary[parameters.DateOfBirth] = new List<FileCabinetRecord>();
            this.dateOfBirthDictionary[parameters.DateOfBirth].Add(record);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.firstNameDictionary.TryGetValue(firstName.ToLower(), out var records)
                ? new ReadOnlyCollection<FileCabinetRecord>(records)
                : new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.lastNameDictionary.TryGetValue(lastName.ToLower(), out var records)
                ? new ReadOnlyCollection<FileCabinetRecord>(records)
                : new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            return this.dateOfBirthDictionary.TryGetValue(dateOfBirth, out var records)
                ? new ReadOnlyCollection<FileCabinetRecord>(records)
                : new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }


    }
}
