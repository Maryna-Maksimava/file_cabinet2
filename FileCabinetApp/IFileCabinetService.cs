using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        static abstract void PrintRecords(ReadOnlyCollection<FileCabinetRecord> records);
        int CreateRecord(RecordParameters parameters);
        void EditRecord(int id, RecordParameters parameters);
        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);
        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);
        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);
        ReadOnlyCollection<FileCabinetRecord> GetRecords();
        int GetStat();
    }
}