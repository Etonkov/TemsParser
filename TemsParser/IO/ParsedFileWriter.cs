using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.IO
{
    public class ParsedFileWriter: IDisposable
    {
        private const int BufferLength = 10000;
        private  StreamWriter _writer;
        private readonly List<string> Lines;
        private readonly string FilePath;
        private readonly object ThisLockObj = new Object();


        public ParsedFileWriter(string filePath, string header)
        {
            FilePath = filePath;
            Lines = new List<string>(BufferLength);
            Lines.Add(header);
            IsInitialized = false;
        }

        public bool IsInitialized { get; private set; }


        private void SaveBuffer()
        {
            lock (Global.LockFileOperations)
            {
                foreach (var lineItem in Lines)
                {
                    _writer.WriteLine(lineItem);
                }
            }

            Lines.Clear();
        }

        public void AddLines(List<string> lines)
        {
            foreach (var lineItem in lines)
            {
                AddLine(lineItem);
            }
        }

        public void AddLine(string line)
        {
            lock (ThisLockObj)
            {
                if (IsInitialized == false)
                {
                    _writer = new StreamWriter(FilePath);
                    IsInitialized = true;
                }

                Lines.Add(line);

                if (Lines.Count == BufferLength)
                {
                    SaveBuffer();
                }
            }
        }


        public void Dispose()
        {
            if (IsInitialized && Lines.Count > 0)
            {
                SaveBuffer();
            }

            if (IsInitialized)
            {
                _writer.Dispose();
            }
        }
    }
}
