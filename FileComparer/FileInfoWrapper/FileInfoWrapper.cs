using System;
using System.IO;

namespace FileComparer
{
    public class FileInfoWrapper
    {
        private Lazy<byte[]> fileHash;

        public FileInfoWrapper(string fullFileName, IFileHashFactoryProvider fileHashFactoryProvider)
        {
            if (fileHashFactoryProvider == null)
            {
                throw new ArgumentNullException();
            }

            if (fullFileName == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                throw new ArgumentException();
            }

            if (!File.Exists(fullFileName))
            {
                throw new FileNotFoundException();
            }

            this.FileInfo = new FileInfo(fullFileName);
            this.fileHash = new Lazy<byte[]>(() => fileHashFactoryProvider.HashFactory(this.FileInfo), isThreadSafe: true);
        }

        public FileInfo FileInfo { get; }

        public byte[] FileHash
        {
            get { return this.fileHash.Value; }
        }

        public override string ToString()
        {
            return this.FileInfo.FullName;
        }
    }
}
