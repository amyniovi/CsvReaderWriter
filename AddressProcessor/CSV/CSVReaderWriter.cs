using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */
    /*
     * async and / or lazy nice to haves...(async will break compatibility as the design stands)
     */

    /*
     * Purpose of this class: To maintain backwards compatibility. 
     * Most responsibilities are moved out of this class, to smaller classes as an attempt to focus on SRP and Open-Closed Principles .
     * The classes I created are not granular enough for example there could be a separate CSVParserService but it d be overengineering at this stage
     * 
     * There are some flaws in how the Legacy system operates at the moment , a very limiting factor was the way a CSVReader
     *  was used external to the API. As it stands it is the user that needs to open() the reader, call read() and then close() it . 
     *  That is very prone to error and also gives responsibility to the user. 
     *  I changed the Reader and Writer to do that internally, but made that transparent to the user to maintain compatibility.
     *  Also Idisposable is not currently implemented. So I implemented IDisposable 
    
     *  I also wrote an Extra ReadLinesAsync  which is not used at the moment, as a future extra.I am using it in a unit test.
     *   There was no time to test multiple read access from different threads and multiple reads while the file is being written to etc..
     *   also no time to test for larger files etc.
     *   But consecutive writes were tested and were successful.
     */
    public class CSVReaderWriter
    {
        private ICsvReader _reader;
        private ICsvWriter _writer;
        private readonly IStreamProvider _provider;

        public CSVReaderWriter()
        {
            _provider = new FileStreamProvider();
        }

        public CSVReaderWriter( IStreamProvider provider)
        {
            _provider = provider;
        }

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, CSVReaderWriter.Mode mode)
        {
            switch (mode)
            {
                case Mode.Read:
                    _reader = new CsvReader(_provider, fileName);
                    break;
                case Mode.Write:
                    _writer = new CsvWriter(_provider,fileName);
                    break;
                default:
                    throw new Exception("unknown file mode for: " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
                _writer.Write(columns);
        }

        public bool Read(string column1, string column2)
        {
            return Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
            column1 = column2 = null;

            var columns = _reader.Read();

            if (columns == null)
                return false;
            var columnArr = columns.ToArray();
            column1 = columnArr[0];
            column2 = columnArr[1];
            return true;

        }

        public void Close()
        {
            //We have to use Dispose() explicitly as the API is used without disposable in mind
            _reader?.Dispose();
            _writer?.Dispose();
        }
    }
}
