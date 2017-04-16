using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV
{
    /// <summary>
    /// This class writes to a stream provided by IStreamProvider
    /// We abstract the streamprovider so we can unit test using a memory stream for example.
    /// </summary>
    public class CsvWriter : ICsvWriter, IDisposable
    {
        private readonly IStreamProvider _provider;
        private StreamWriter _streamWriter;
        private readonly string _filename;
        private string _separator;
        public Stream Stream { get; set; }
        public bool IsDisposed;

        public CsvWriter(IStreamProvider provider, string filename, string separator = "\t")
        {
            _provider = provider;
            _filename = filename;
            _separator = separator;
            Stream = _provider.OpenStream(CSVReaderWriter.Mode.Write, _filename);
            _streamWriter = new StreamWriter(Stream, Encoding.Default);
        }

        //overwrites text , we should also create methods to append text etc    
        public void Write(IEnumerable<string> columns)
        {
            var outPut = string.Join(_separator, columns);
            _streamWriter.WriteLine(outPut);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //if disposing false, called by finilizer
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _streamWriter?.Close();
            _streamWriter?.Dispose();
            Stream?.Close();
            Stream?.Dispose();
            IsDisposed = true;
        }
        ~CsvWriter()
        {
            if (IsDisposed) return;
            Dispose();
        }
    }
}