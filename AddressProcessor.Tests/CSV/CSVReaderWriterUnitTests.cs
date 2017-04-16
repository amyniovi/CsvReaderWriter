using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using AddressProcessing.CSV;
using AddressProcessing.CSV.Interfaces;
using Moq;
using NUnit.Framework;

namespace Csv.Tests
{
    /// <summary>
    /// This class tests that refactored CSVReaderWriter can write and read from an abstracted stream as expected, 
    /// when used exactly as in the legacy system(backwards compatibility)
    /// </summary>
    [TestFixture]
    public class CSVReaderWriterUnitTests
    {
        private CSVReaderWriter.Mode _readMode = CSVReaderWriter.Mode.Read;
        private CSVReaderWriter.Mode _writeMode = CSVReaderWriter.Mode.Write;
        // private readonly IStreamProvider _streamProvider = new FileStreamProvider();
        private string _separator = "\t";
        private  Mock<IStreamProvider> _providerMock;
        private Mock<ICsvReader> _readerMock;
        private Mock<ICsvWriter> _writerMock;
        private Stream _readMemoryStream = null;
        private Stream _writeMemoryStream = null;

        [SetUp]
        public void SetUp()
        {
            _writerMock = new Mock<ICsvWriter>();
            _readerMock = new Mock<ICsvReader>();
            _providerMock = new Mock<IStreamProvider>();
            _readMemoryStream = GetMemoryStreamFromString("Amy\tAddressLine1|W12\r\n");
            _providerMock.Setup(p => p.OpenStream(_readMode, It.IsAny<string>())).Returns(_readMemoryStream);
            _writeMemoryStream = GetMemoryStreamFromString("");
            _providerMock.Setup(p => p.OpenStream(_writeMode, It.IsAny<string>())).Returns(_writeMemoryStream);
        }

        [TearDown]
        public void TearDown()
        {
            _readerMock  = null;
            _providerMock = null;
            _writerMock = null;
            _readMemoryStream = _writeMemoryStream = null;
        }

        [Test]
        public void Read_WhenValidStream_ParsesAndReadsLineCorrectly()
        {
            var readerWriter = new CSVReaderWriter( _providerMock.Object);
            readerWriter.Open("", _readMode);
            string column1, column2;
            readerWriter.Read(out column1, out column2);
            Assert.That(column1 == "Amy");
            Assert.That(column2 == "AddressLine1|W12");
        }

        [Test]
        public void Read_WhenValidFile_OpenStreamIsCalledOnce()
        {
            var readerWriter = new CSVReaderWriter( _providerMock.Object);
            readerWriter.Open("", _readMode);
            string column1, column2;
            readerWriter.Read(out column1, out column2);
            _providerMock.Verify(p => p.OpenStream(_readMode, It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Write_WhenValidStream_OpenStreamIsalledOnce()
        {
            var readerWriter = new CSVReaderWriter(_providerMock.Object);
            readerWriter.Open("", _writeMode);
            readerWriter.Write(new string[] { "Amy", "AddressLine1|W12" });
            _providerMock.Verify(p => p.OpenStream(_writeMode, It.IsAny<string>()), Times.Once);
        }
        
        [Test]
        [ExpectedException(typeof(System.ObjectDisposedException))]
        public void Read_AfterReadComplete_CheckUnmanagedResourcesDisposed()
        {
            var reader = new CSVReaderWriter(_providerMock.Object);
            reader.Open("", _readMode);
            string column1, column2;
            string column3, column4;
            reader.Read(out column1, out column2);
            reader.Close();
            
             reader.Read(out column3, out column4); 
        }

        [Test]
        [ExpectedException(typeof(System.ObjectDisposedException))]
        public void Write_AfterWriteComplete_CheckUnmanagedResourcesDisposed()
        {
            var writer = new CSVReaderWriter( _providerMock.Object);
            writer.Open("", _writeMode);
            writer.Write(new string[] { "Amy", "AddressLine1|W12" });
            writer.Close();

            //trying to use a disposed reader
            writer.Write(new string[] { "Amy", "AddressLine1|W12" });
        }

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
