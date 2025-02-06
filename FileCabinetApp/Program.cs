using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Maryna Maksimava";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService? fileCabinetService;
        private static bool isRunning = true;

        // COMMANDS
        private static readonly Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        // MESSAGES
        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "does stat", "stat stat stat." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "list", "shows records", "The 'list' command shows the list of records." },
            new string[] { "edit", "edits a record", "The 'edit' command edits a record." },
            new string[] { "find", "finds a record", "The 'find' command finds a record." },
        };

        static Program()
        {
            fileCabinetService = new FileCabinetService();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', (char)2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    //var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    var parameters = inputs.Length > 1  ? string.Join(" ", inputs.Skip(1)) : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
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

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        //--------------------------------------------------------------------------------------------
        private static void Create(string parameters)
        {
            string? fname = string.Empty;
            string? lname = string.Empty;
            DateTime dateOfBirth = DateTime.MinValue;
            short age = 0;
            decimal salary = 0;
            char gender = ' ';

            // Fname check
            while (true)
            {
                Console.WriteLine("First name...");
                fname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(fname) || fname.Length < 2 || fname.Length > 60)
                {
                    Console.WriteLine("First name must be between 2 and 60 characters and cannot be empty or just spaces.");
                }
                else
                {
                    break;
                }
            }

            // Lname check
            while (true)
            {
                Console.WriteLine("Last name...");
                lname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(lname) || lname.Length < 2 || lname.Length > 60)
                {
                    Console.WriteLine("Last name must be between 2 and 60 characters and cannot be empty or just spaces.");
                }
                else
                {
                    break;
                }
            }

            // DOB check
            while (true)
            {
                Console.WriteLine("Date of birth (mm.dd.yyyy)...");
                string date = Console.ReadLine();
                if (DateTime.TryParse(date, out dateOfBirth) && dateOfBirth >= new DateTime(1900, 1, 1) && dateOfBirth <= DateTime.Today)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date. Please enter a valid date between 01-Jan-1900 and today.");
                }
            }

            // age check
            while (true)
            {
                Console.WriteLine("Age...");
                if (short.TryParse(Console.ReadLine(), out age) && age > 0 && age <= 130)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Age must be a positive value between 1 and 130.");
                }
            }

            // salary check
            while (true)
            {
                Console.WriteLine("Salary...");
                if (decimal.TryParse(Console.ReadLine(), out salary) && salary >= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Salary must be a non-negative number.");
                }
            }

            // gender check
            while (true)
            {
                Console.WriteLine("Gender (M/F/O(for Other)...");
                string? genderInput = Console.ReadLine();
                if (genderInput.Length == 1 && (genderInput[0] == 'M' || genderInput[0] == 'F' || genderInput[0] == 'O'))
                {
                    gender = genderInput[0];
                    break;
                }
                else
                {
                    Console.WriteLine("Gender must be 'M' or 'F' or 'O'.");
                }
            }

            int id = fileCabinetService.CreateRecord(fname, lname, dateOfBirth, age, salary, gender);

            Console.WriteLine($"record id = {id}");
        }

        //---------------------------------------------------------------------------------------------------------

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();

            if (records.Length == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, Age: {record.Age}, Salary: {record.Salary:C}, Gender: {record.Gender}");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
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

            string fname = string.Empty;
            string lname = string.Empty;
            DateTime dateOfBirth = DateTime.MinValue;
            short age = 0;
            decimal salary = 0;
            char gender = ' ';

            //Fname
            while (true)
            {
                Console.WriteLine("New first name...");
                fname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(fname) || fname.Length < 2 || fname.Length > 60)
                {
                    Console.WriteLine("First name must be between 2 and 60 characters and cannot be empty or just spaces.");
                }
                else
                {
                    break;
                }
            }

            // Lname
            while (true)
            {
                Console.WriteLine("New last name...");
                lname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(lname) || lname.Length < 2 || lname.Length > 60)
                {
                    Console.WriteLine("Last name must be between 2 and 60 characters and cannot be empty or just spaces.");
                }
                else
                {
                    break;
                }
            }

            // DOB
            while (true)
            {
                Console.WriteLine("New date of birth (mm.dd.yyyy)...");
                string date = Console.ReadLine();
                if (DateTime.TryParse(date, out dateOfBirth) && dateOfBirth >= new DateTime(1950, 1, 1) && dateOfBirth <= DateTime.Today)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date. Please enter a valid date between 01-Jan-1950 and today.");
                }
            }

            // age
            while (true)
            {
                Console.WriteLine("New age...");
                if (short.TryParse(Console.ReadLine(), out age) && age > 0 && age <= 130)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Age must be a positive value between 1 and 130.");
                }
            }

            // salary
            while (true)
            {
                Console.WriteLine("New salary...");
                if (decimal.TryParse(Console.ReadLine(), out salary) && salary >= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Salary must be a non-negative number.");
                }
            }

            // gender
            while (true)
            {
                Console.WriteLine("New gender (M/F/O(for Other)...");
                string genderInput = Console.ReadLine();
                if (genderInput.Length == 1 && (genderInput[0] == 'M' || genderInput[0] == 'F' || genderInput[0] == 'O'))
                {
                    gender = genderInput[0];
                    break;
                }
                else
                {
                    Console.WriteLine("Gender must be 'M' or 'F' or 'O'.");
                }
            }

            try
            {
                fileCabinetService.EditRecord(id, fname, lname, dateOfBirth, age, salary, gender);
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

    }
}
