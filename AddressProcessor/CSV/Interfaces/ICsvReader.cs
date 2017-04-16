using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace AddressProcessing.CSV.Interfaces
{
    public interface ICsvReader : IDisposable
    {
        IEnumerable<string> Read();
        Stream Stream { get; set; }
    }
}