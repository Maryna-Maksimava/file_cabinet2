using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlWriter
    {
        private readonly TextWriter writer;
        private readonly XmlWriter xmlWriter;

        public FileCabinetRecordXmlWriter(TextWriter writer)
        {
            this.writer = writer;
            var settings = new XmlWriterSettings { Indent = true };
            this.xmlWriter = XmlWriter.Create(writer, settings);
            this.xmlWriter.WriteStartDocument();
            this.xmlWriter.WriteStartElement("records");
        }

        public void Write(FileCabinetRecord record)
        {
            this.xmlWriter.WriteStartElement("record");
            this.xmlWriter.WriteAttributeString("id", record.Id.ToString());

            this.xmlWriter.WriteStartElement("name");
            this.xmlWriter.WriteAttributeString("first", record.FirstName);
            this.xmlWriter.WriteAttributeString("last", record.LastName);
            this.xmlWriter.WriteEndElement();

            this.xmlWriter.WriteElementString("dateOfBirth", record.DateOfBirth.ToString("MM/dd/yyyy"));
            this.xmlWriter.WriteElementString("age", record.Age.ToString());
            this.xmlWriter.WriteElementString("salary", record.Salary.ToString());
            this.xmlWriter.WriteElementString("gender", record.Gender.ToString());

            this.xmlWriter.WriteEndElement(); 
        }

        public void Close()
        {
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.WriteEndDocument();
            this.xmlWriter.Close();
        }
    }
}
