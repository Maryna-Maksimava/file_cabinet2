// <copyright file="FileCabinetRecordXmlWriter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileCabinetApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    /// <summary>
    /// Provides functionality to write FileCabinetRecord objects to XML format.
    /// </summary>
    internal class FileCabinetRecordXmlWriter
    {
        private readonly TextWriter writer;
        private readonly XmlWriter xmlWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">The text writer to which the XML content will be written.</param>
        public FileCabinetRecordXmlWriter(TextWriter writer)
        {
            this.writer = writer;
            var settings = new XmlWriterSettings { Indent = true };
            this.xmlWriter = XmlWriter.Create(writer, settings);
            this.xmlWriter.WriteStartDocument();
            this.xmlWriter.WriteStartElement("records");
        }

        /// <summary>
        /// Writes a single record to the XML document.
        /// </summary>
        /// <param name="record">The record to write to XML format.</param>
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

        /// <summary>
        /// Finalizes the XML document and closes the XML writer.
        /// </summary>
        public void Close()
        {
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.WriteEndDocument();
            this.xmlWriter.Close();
        }
    }
}
