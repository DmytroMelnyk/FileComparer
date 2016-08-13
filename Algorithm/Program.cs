using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Algorithm
{
    class Program
    {
        // E.g. for args: "C:\" "C:\temp.txt" "C:\Windows" ....
        // Any number of files and directories
        static void Main(string[] args)
        {
            var equalFiles = args.Where(path => Directory.Exists(path))
                .SelectMany(path => Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                .Concat(args.Where(path => File.Exists(path)))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(fileName => new FileInfo(fileName))
                .GroupBy(item => item.Length)
                .Where(item => item.Skip(1).Any())
                .SelectMany(item => item)
                .AsParallel()
                .Select(item =>
                {
                    using (var fs = item.OpenRead())
                        return new { FileInfo = item, Hash = MD5.Create().ComputeHash(fs) };
                })
                .GroupBy(item => item.Hash, ArrayComparer<byte[]>.Default)
                .Where(item => item.Skip(1).Any())
                .Select(item => item.Select(subItem => subItem.FileInfo).ToList())
                .ToList();
        }
    }
}
