using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV
{
    public class CsvReader : ICsvReader, IDisposable
    {
        private readonly IStreamProvider _provider;
        private readonly string _filename;
        private readonly char[] _separators;
        public StreamReader _readerStream { get; set; }
        public Stream Stream { get; set; }
        public string Separator { get; set; } = "\t";
        public bool IsDisposed { get; set; }

        public CsvReader(IStreamProvider provider, string filename, char[] separators = null)
        {
            _provider = provider;
            _filename = filename;
            _separators = separators == null || separators == new char[0] ? new char[] { '\t' } : separators;
            Stream = _provider.OpenStream(CSVReaderWriter.Mode.Read, _filename);
            _readerStream = new StreamReader(Stream, Encoding.Default);
        }

        public IEnumerable<string> Read()
        {
            var line = _readerStream.ReadLine();

            return line?.Split(_separators).ToArray();
        }

        //This can be the future implementation of Read(), where besides it being async it reads all the lines,
        //thus not expecting the user to loop through
        public async Task<IEnumerable<string>> ReadAllLines()
        {
            List<string> result = new List<string>();

            while (!_readerStream.EndOfStream)
            {
                var line = await _readerStream.ReadLineAsync();
                if (line == null)
                    throw new Exception("Error Reading from Stream");
                var lineColumns = line.Split(_separators).ToList().Take(2).ToArray();
                result.Add(string.Join(Separator, lineColumns));
            }

            return result;
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
            _readerStream?.Close();
            _readerStream?.Dispose();
            Stream?.Close();
            Stream?.Dispose();
            IsDisposed = true;
        }
        ~CsvReader()
        {
            if (IsDisposed) return;
            Dispose();
        }
    }
}