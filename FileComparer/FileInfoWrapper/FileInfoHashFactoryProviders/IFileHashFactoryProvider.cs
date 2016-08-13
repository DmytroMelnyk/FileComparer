using System;
using System.IO;

namespace FileComparer
{
    public interface IFileHashFactoryProvider
    {
        Func<FileInfo, byte[]> HashFactory { get; }
    }
}
