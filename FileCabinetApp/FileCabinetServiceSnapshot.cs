using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly List<FileCabinetRecord> records;

        public FileCabinetServiceSnapshot(List<FileCabinetRecord> records)
        {
            this.records = new List<FileCabinetRecord>(records);
        }

        public IReadOnlyCollection<FileCabinetRecord> Records()
        {
            return this.records.AsReadOnly();
        }

        public void SaveToCsv(StreamWriter writer)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(writer);

            // Write header
            writer.WriteLine("Id,First Name,Last Name,Date of Birth,Age,Salary,Gender");

            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        public void SaveToXml(StreamWriter writer)
        {
           // var xmlWriter = new FileCabinetRecordXmlWriter(writer);

            foreach (var record in this.records)
            {
            //    xmlWriter.Write(record);
            }
        }
    }
}
