using System;
using System.IO;
using AddressProcessing.CSV.Interfaces;
using AddressProcessing.CSV.Strategies;

namespace AddressProcessing.CSV
{
    public class FileStreamProvider : IStreamProvider
    {
        public Stream OpenStream(CSVReaderWriter.Mode mode, string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException();

            if (!Enum.IsDefined(typeof(CSVReaderWriter.Mode), mode))
            {
                throw new Exception("Unknown file mode for " + filename);
            }
            Func<string, FileStream> getFilestream;
            FileStreamProviderStrategies.Strategies.TryGetValue(mode, out getFilestream);
            return getFilestream?.Invoke(filename);
        }
    }
}