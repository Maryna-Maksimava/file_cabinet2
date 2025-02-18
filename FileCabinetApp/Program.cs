﻿using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Entry point of the File Cabinet Application.
    /// </summary>
    public class Program
    {
        private const string DeveloperName = "Maryna Maksimava";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static IFileCabinetService? fileCabinetService;
        private static bool isRunning = true;

        static Program()
        {
            fileCabinetService = new FileCabinetService(new DefaultValidator());
        }

        /// <summary>
        /// List of available commands and their actions.
        /// </summary>
        private static readonly Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
        };

        /// <summary>
        /// Help messages for available commands.
        /// </summary>
        private static readonly string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "provides statistics", "The 'stat' command displays record statistics." },
            new string[] { "create", "creates new record", "The 'create' command creates a new record." },
            new string[] { "list", "shows records", "The 'list' command displays all records." },
            new string[] { "edit", "edits a record", "The 'edit' command modifies an existing record." },
            new string[] { "find", "finds a record", "The 'find' command searches for a record." },
            new string[] { "export", "export records to CSV", "The 'export' command exports records to CSV"},
        };

        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            // parse validation type
            string validationType = "default"; // Default value

            for (int i = 0; i < args.Length; i++)
            {
                var argument = args[i].ToLower();

                if (argument.StartsWith("--validation-rules="))
                {
                    validationType = argument.Split('=')[1];
                }
                if (argument == "-v" && !string.IsNullOrEmpty(args[i + 1]))
                {
                    validationType = args[i + 1].ToLower();
                }
            }
            
            if (validationType == "custom")
            {
                Console.WriteLine("Using custom validation rules.");
                fileCabinetService = new FileCabinetService(new CustomValidator());
            }
            else
            {
                Console.WriteLine("Using default validation rules.");
                fileCabinetService = new FileCabinetService(new DefaultValidator());
            }


            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line?.Split(' ', (char)2) ?? new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? string.Join(" ", inputs.Skip(1)) : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }


        /// <summary>
        /// Displays message when an unknown command is entered.
        /// </summary>
        /// <param name="command">The unknown command.</param>
        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        /// <summary>
        /// Displays available commands or provides help for a specific command.
        /// </summary>
        /// <param name="parameters">Command to get help for.</param>
        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");
                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <param name="parameters">Unused parameter.</param>
        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting the application...");
            isRunning = false;
        }

        /// <summary>
        /// Displays statistics about stored records.
        /// </summary>
        /// <param name="parameters">Unused parameter.</param>
        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService?.GetStat() ?? 0;
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static Tuple<bool, string, string> stringConverter(string input) {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create(false, "Empty string", "");
            }

            return Tuple.Create(true, string.Empty, input);
        }

        private static Tuple<bool, string> firstNameValidator(string input)
        {
            if (input.Length < 2 )
            {
                return Tuple.Create(false, "First name too short. Minimum 2 symbols");
            }
            if (input.Length > 60)
            {
                return Tuple.Create(false, "First name too long. Maximum 60 symbols");
            }

            return Tuple.Create(true, string.Empty);
        }

        private static Tuple<bool, string> lastNameValidator(string input)
        {
            if (input.Length < 2)
            {
                return Tuple.Create(false, "Last name too short. Minimum 2 symbols");
            }
            if (input.Length > 60)
            {
                return Tuple.Create(false, "Last name too long. Maximum 60 symbols");
            }

            return Tuple.Create(true, string.Empty);
        }

        private static Tuple<bool, string, DateTime> dateConverter(string input)
        {
            if (DateTime.TryParse(input, out DateTime result))
            {
                return Tuple.Create(true, string.Empty, result);
            }
            return Tuple.Create(false, "Invalid date format. Use mm.dd.yyyy", DateTime.MinValue);
        }

        private static Tuple<bool, string> dateValidator(DateTime input)
        {
            if (input < new DateTime(1900, 1, 1))
            {
                return Tuple.Create(false, "Date must be after 01.01.1900");
            }
            if (input > DateTime.Today)
            {
                return Tuple.Create(false, "Date cannot be in the future");
            }
            return Tuple.Create(true, string.Empty);
        }

        private static Tuple<bool, string, decimal> salaryConverter(string input)
        {
            if (decimal.TryParse(input, out decimal result))
            {
                return Tuple.Create(true, string.Empty, result);
            }
            return Tuple.Create(false, "Please enter a valid number", 0m);
        }

        private static Tuple<bool, string> salaryValidator(decimal input)
        {
            if (input < 0)
            {
                return Tuple.Create(false, "Salary must be a non-negative number");
            }
            return Tuple.Create(true, string.Empty);
        }

        private static Tuple<bool, string, short> ageConverter(string input)
        {
            if (short.TryParse(input, out short result))
            {
                return Tuple.Create(true, string.Empty, result);
            }
            return Tuple.Create(false, "Please enter a valid number", (short)0);
        }

        private static Tuple<bool, string> ageValidator(short input)
        {
            if (input <= 0)
            {
                return Tuple.Create(false, "Age must be positive");
            }
            if (input > 130)
            {
                return Tuple.Create(false, "Age must be less than or equal to 130");
            }
            return Tuple.Create(true, string.Empty);
        }

        private static Tuple<bool, string, char> genderConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 1)
            {
                return Tuple.Create(false, "Please enter a single character", ' ');
            }
            char gender = char.ToUpper(input[0]);
            return Tuple.Create(true, string.Empty, gender);
        }

        private static Tuple<bool, string> genderValidator(char input)
        {
            char gender = char.ToUpper(input);
            if (gender != 'M' && gender != 'F' && gender != 'O')
            {
                return Tuple.Create(false, "Gender must be 'M' or 'F' or 'O'");
            }
            return Tuple.Create(true, string.Empty);
        }

        private static RecordParameters ReadRecordParameters()
        {
            string fname = string.Empty;
            string lname = string.Empty;
            DateTime dateOfBirth = DateTime.MinValue;
            short age = 0;
            decimal salary = 0;
            char gender = ' ';

            // Fname check
            Console.WriteLine("First name...");
            fname = ReadInput(stringConverter, firstNameValidator);

            // Lname check

            Console.WriteLine("Last name...");
            lname = ReadInput(stringConverter, lastNameValidator);

            // DOB check
            Console.WriteLine("Date of birth (mm.dd.yyyy)...");
            dateOfBirth = ReadInput(dateConverter, dateValidator);



            // age check
            Console.WriteLine("Age...");
            age = ReadInput(ageConverter, ageValidator);

            // salary check
            Console.WriteLine("Salary...");
            salary = ReadInput(salaryConverter, salaryValidator);

            // gender check
            Console.WriteLine("Gender (M/F/O(for Other)...");
            gender = ReadInput(genderConverter, genderValidator);

            return new RecordParameters(fname, lname, dateOfBirth, age, salary, gender);
        }


        private static void Create(string parameters)
        {
            RecordParameters recParams = ReadRecordParameters();

            int id = fileCabinetService.CreateRecord(recParams);
            Console.WriteLine($"record id = {id}");
        }


        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();

            if (records.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, Age: {record.Age}, Salary: {record.Salary:C}, Gender: {record.Gender}");
            }
        }
        private static void Edit(string parameters)
        {
            // id
            Console.WriteLine("Enter the record ID to edit...");
            int id;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out id) && id > 0)
                {
                    break;
                }

                Console.WriteLine("Please enter a valid positive record ID.");
            }

            RecordParameters recParams = ReadRecordParameters();

            try
            {
                fileCabinetService.EditRecord(id, recParams);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Find(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Usage: find <property> <value>");
                return;
            }

            var parts = parameters.Split(separator: new[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                Console.WriteLine("Usage: find <property> <value>");
                return;
            }

            string propertyName = parts[0].ToLower();
            string searchValue = parts[1];

            switch (propertyName)
            {
                case "firstname":
                    var recordsByFirstName = fileCabinetService.FindByFirstName(searchValue);
                    FileCabinetService.PrintRecords(recordsByFirstName);
                    break;

                case "lastname":
                    var recordsByLastName = fileCabinetService.FindByLastName(searchValue);
                    FileCabinetService.PrintRecords(recordsByLastName);
                    break;

                case "dateofbirth":
                    if (DateTime.TryParse(searchValue, out DateTime dob))
                    {
                        var recordsByDob = fileCabinetService.FindByDateOfBirth(dob);
                        FileCabinetService.PrintRecords(recordsByDob);
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Use YYYY-MM-DD.");
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown property '{propertyName}'. Use: firstname, lastname, dateofbirth.");
                    break;
            }
        }

        private static void Export(string parameters)
        {
            FileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();

            var parts = parameters.Split(' ', 2);
            if (parts.Length != 2)
            {
                Console.WriteLine("export csv[or xml] filename.csv");
                return;
            }

            var type = parts[0].ToLower();
            var fileName = parts[1];

            StreamWriter? writer = null;

            try
            {
                writer = new StreamWriter(fileName);

                if (type == "csv") {
                    snapshot.SaveToCsv(writer);
                }
                else if (type == "xml") {
                    snapshot.SaveToXml(writer);
                } else {
                    Console.WriteLine("Unsupported file type");
                }
                
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (writer != null) {
                writer.Close();
            }
        }
    }
}
