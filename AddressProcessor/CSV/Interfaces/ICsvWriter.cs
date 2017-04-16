using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace AddressProcessing.CSV.Interfaces
{
    public interface ICsvWriter :IDisposable
    {
        Stream Stream { get; set; }
        void Write(IEnumerable<string> columns);
    }
}