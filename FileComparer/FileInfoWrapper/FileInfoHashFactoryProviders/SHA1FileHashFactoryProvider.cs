using System;
using System.IO;
using System.Security.Cryptography;

namespace FileComparer
{
    public class SHA1FileHashFactoryProvider : IFileHashFactoryProvider
    {
        public Func<FileInfo, byte[]> HashFactory { get; } = fileInfo =>
        {
            using (var fileStream = fileInfo.OpenRead())
            {
                return SHA1.Create().ComputeHash(fileStream);
            }
        };
    }
}
