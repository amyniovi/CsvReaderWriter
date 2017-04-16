using System;
using System.IO;

namespace AddressProcessing.CSV.Interfaces
{
    public interface IStreamProvider 
    {
        Stream OpenStream(CSVReaderWriter.Mode mode, string filename = null);
    }
}