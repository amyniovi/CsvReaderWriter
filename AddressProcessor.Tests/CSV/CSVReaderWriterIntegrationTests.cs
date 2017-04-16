using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AddressProcessing.CSV;
using AddressProcessing.CSV.Interfaces;
using Microsoft.SqlServer.Server;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    /// <summary>
    /// This class tests that refactored CSVReaderWriter can write and read files as expected, 
    /// when used exactly as in the legacy system(backwards compatibility)
    /// </summary>
    [TestFixture]
    class CSVReaderWriterIntegrationTests
    {
        private string _fileNameForReads = @"test_data\contacts.csv";
        private string _fileNameForWrites = @"test_data\contacts2.csv";
        private CSVReaderWriter.Mode _readMode = CSVReaderWriter.Mode.Read;
        private CSVReaderWriter.Mode _writeMode = CSVReaderWriter.Mode.Write;
        private string _separator = "\t";
        private char[] _separators = { '\t' };

        [TearDown]
        public void TearDown()
        {
            File.WriteAllLines(_fileNameForWrites, new List<string>());
        }
        [Test]
        public void ReadOneLine_WhenFileValid_AndReadMode_CheckColumnsCorrect()
        {
            string name;
            string address;

            var reader = new CSVReaderWriter();
            reader.Open(_fileNameForReads, _readMode);
            reader.Read(out name, out address);
            reader.Close();

            Assert.That(name == "Shelby Macias");
            Assert.That(address == "3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England");
        }

        [Test]
        public void ReadAllLines_WhenFileValid_CheckColumnsCorrect()
        {
            string name;
            string address;
            var result = new List<string>();

            var reader = new CSVReaderWriter();

            reader.Open(_fileNameForReads, _readMode);
            while (reader.Read(out name, out address))
            {
                var values = new string[] { name, address };
                result.Add(string.Join(_separator, values));
            }
            reader.Close();

            var allLines = File.ReadAllLines(_fileNameForReads).ToList();
            var expectedCollection = new List<string>();

            allLines.ForEach(line =>
            {
                var stringList = line.Split(_separators).ToList().Take(2);
                expectedCollection.Add(string.Join(_separator, stringList));
            });

            CollectionAssert.AreEqual(expectedCollection, result);
        }

        [Test]
        public void Write_WhenFileValid_AndWriteMode_CheckColumnsCorrect()
        {
            var writer = new CSVReaderWriter();

            writer.Open(_fileNameForWrites, _writeMode);
            writer.Write(new string[] { "Amy", "AddressLine1|W12" });
            writer.Close();
            var allTheText = File.ReadAllText(_fileNameForWrites);

            Assert.That(allTheText, Is.EqualTo("Amy\tAddressLine1|W12\r\n"));
        }

        [Test]
        [ExpectedException()]
        public void WriteTwice_ValidFile_FileOverwritten()
        {
            var filename = _fileNameForWrites;
            var mode = CSVReaderWriter.Mode.Write;

            var writer = new CSVReaderWriter();
            writer.Open(filename, mode);
            writer.Write(new string[] { "Dwayne", "AddressLine1|W10" });
            writer.Close();
            writer.Open(filename, mode);
            writer.Write(new string[] { "Amy", "AddressLine1|W12" });
            writer.Close();
            Assert.That(File.ReadAllText(filename) == "Amy\tAddressLine1|W12\r\n");
        }

        [Test]
        public void MultipleReads_DisposableImplemented_Succcess()
        {
            var filename = @"test_data\contacts.csv";
            var mode = CSVReaderWriter.Mode.Read;
            List<Task<List<string>>> tasks = new List<Task<List<string>>>();
            for (Int64 i = 0; i < 20; i++)
            {
                tasks.Add(new Task<List<string>>(() =>
                {
                    List<string> result = new List<string>();
                    var reader = new CSVReaderWriter();

                    string column1;
                    string column2;
                    reader.Open(filename, mode);
                    reader.Read(out column1, out column2);
                    result.Add(column1);
                    result.Add(column2);

                    return result;

                }));
            }

            foreach (var task in tasks)
            {
                task.Start();
                task.ContinueWith(t => Assert.That(t.Result[0] == "Shelby Macias" && t.Result[1] == "3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England"));
            }

            Task.WaitAll();
        }

        [Test]
        [ExpectedException()]
        public void Read_WhenFileDoesNotExist_ExceptionThrown()
        {
            var filename = @"test_data\contacts#.csv";

            var reader = new CSVReaderWriter();

            reader.Open(filename, _readMode);
            string column1;
            string column2;
            reader.Read(out column1, out column2);

        }

        [Test]
        public void Write_WhenFileDoesNotExist_CreateIt()
        {
            var filename = @"test_data\contacts##.csv";

            var reader = new CSVReaderWriter();

            reader.Open(filename, _writeMode);
            reader.Write(new string[] { "Amy", "AddressLine1|W12" });

            Assert.That(File.Exists(filename));
        }
    }
}



