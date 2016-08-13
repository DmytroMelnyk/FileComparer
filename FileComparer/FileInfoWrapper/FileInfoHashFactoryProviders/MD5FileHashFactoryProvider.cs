using System;
using System.IO;
using System.Security.Cryptography;

namespace FileComparer
{
    public class MD5FileHashFactoryProvider : IFileHashFactoryProvider
    {
        public Func<FileInfo, byte[]> HashFactory { get; } = fileInfo =>
        {
            using (var fileStream = fileInfo.OpenRead())
            {
                return MD5.Create().ComputeHash(fileStream);
            }
        };
    }
}
