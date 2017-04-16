using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressProcessing.CSV;
using AddressProcessing.CSV.Interfaces;
using Moq;
using NUnit.Framework;


namespace AddressProcessing.Tests
{
    /// <summary>
    /// This class demonstrates that our refactored system (CsvReader, CsvWriter, FileStreamProvider) 
    /// can work more efficiently as a standalone system, and has an easier more flexible design .
    /// These tests do not involve CSVReaderWriter
    /// </summary>
    [TestFixture]
    class NewCsvReader_CsvWriter_UnitTests

    {
        private CSVReaderWriter.Mode _readMode = CSVReaderWriter.Mode.Read;
        private CSVReaderWriter.Mode _writeMode = CSVReaderWriter.Mode.Write;
        private string _separator = "\t";
        private char[] _separators = { '\t' };
        private Mock<IStreamProvider> _providerMock;
        private Stream _readMemoryStream = null;
        private Stream _writeMemoryStream = null;

        [SetUp]
        public void SetUp()
        {
            _providerMock = new Mock<IStreamProvider>();
            _readMemoryStream = GetMemoryStreamFromString("Amy\tAddressLine1|W12\r\n");
            _providerMock.Setup(p => p.OpenStream(_readMode, It.IsAny<string>())).Returns(_readMemoryStream);
            _writeMemoryStream = new MemoryStream();
            _providerMock.Setup(p => p.OpenStream(_writeMode, It.IsAny<string>())).Returns(_writeMemoryStream);
        }
        [TearDown]
        public void TearDown()
        {
            _providerMock = null;
            _readMemoryStream = _writeMemoryStream = null;
        }

        [Test]
        public void Read_WhenValidStream_ParsesAndReadsLineCorrectly()
        {
            IEnumerable<string> list = new List<string>();
            using (var reader = new CsvReader(_providerMock.Object, ""))
            {
                list = reader.Read();
            }

            Assert.That(list.ToArray()[0] == "Amy");
            Assert.That(list.ToArray()[1] == "AddressLine1|W12");
        }

        [Test]
        public void Read_WhenValiStream_OpenStreamIsCalledOnce()
        {
            using (var reader = new CsvReader(_providerMock.Object, ""))
            {
                reader.Read();
            }

            _providerMock.Verify(p => p.OpenStream(_readMode, It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Read_TestFile_ReadAllLinesSuccess()
        {
            List<string> actualLines;
            using (var reader = new CsvReader(new FileStreamProvider(), @"test_data\contacts.csv"))
            {
                var tasklines = reader.ReadAllLines();
                actualLines = tasklines.Result.ToList();
            }

            var allLines = File.ReadAllLines(@"test_data\contacts.csv").ToList();
            var expectedCollection = new List<string>();

            allLines.ForEach(line =>
            {
                var stringList = line.Split(_separators).ToList().Take(2);
                expectedCollection.Add(string.Join(_separator, stringList));
            });

            CollectionAssert.AreEqual(expectedCollection, actualLines);
        }

        [Test]
        public void Write_WhenValidStream_OpenStreamIsalledOnce()
        {
            using (var writer = new CsvWriter(_providerMock.Object, ""))
            {
                writer.Write(new string[] { "Amy", "AddressLine1|W12" });
            }

            _providerMock.Verify(p => p.OpenStream(_writeMode, It.IsAny<string>()), Times.Once);
        }

        [Test]

        public void Read_AfterReadComplete_CheckUnmanagedResourcesDisposed()
        {
            var reader = new CsvReader(_providerMock.Object, "");
            using (reader)
            {
                reader.Read();
            }
            Assert.IsTrue(reader.IsDisposed);
        }

        [Test]
        public void Write_AfterWriteComplete_CheckUnmanagedResourcesDisposed()
        {
            var writer = new CsvWriter(_providerMock.Object, "");
            using (writer)
            {
                writer.Write(new string[] { "Amy", "AddressLine1|W12" });
            }

           Assert.IsTrue(writer.IsDisposed);
        }

        //[Test]
        //public void MultipleWrites_DisposableImplemented_Succcess()
        //{
        //    var filename = @"test_data\contacts2.csv";
        //    var mode = CSVReaderWriter.Mode.Write;
        //    List<Task> tasks = new List<Task>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        tasks.Add(new Task(() =>
        //        {
        //            using (var writer = new CsvWriter(new FileStreamProvider(), filename))
        //            {
        //                writer.Write(new List<string> { "Amy", "AddressLine1|W12" });
        //            }
        //        }));
        //    }

        //    foreach (var task in tasks)
        //    {
        //        task.Start();
        //    }
        //    Task.WaitAll();
        //    var text = File.ReadAllText(filename);
        //    Assert.That(text == @"Amy\tAddressLine1|W12\r\");
        //}
        private static MemoryStream GetMemoryStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var sw = new StreamWriter(stream);
            sw.Write(s);
            sw.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
