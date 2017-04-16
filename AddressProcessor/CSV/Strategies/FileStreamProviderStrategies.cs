using System;
using System.Collections.Generic;
using System.IO;

namespace AddressProcessing.CSV.Strategies
{
    //Can be extended to read from multiple streams memory or file etc (Stream instead of FileStream)
    //mode enum can later be a standalone file, could add ReadWrite modes etc
    //overengineering as system stands
    public static class FileStreamProviderStrategies
    {
        public static Dictionary<CSVReaderWriter.Mode, Func<string, FileStream>> Strategies =
            new Dictionary<CSVReaderWriter.Mode, Func<string, FileStream>>();

        static FileStreamProviderStrategies()
        {
            Strategies.Add(CSVReaderWriter.Mode.Read, File.OpenRead);

            Strategies.Add(CSVReaderWriter.Mode.Write, File.OpenWrite);
        }
    }
}