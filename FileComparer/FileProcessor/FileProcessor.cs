using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace FileComparer
{
    public class FileProcessor
    {
        private readonly Lazy<List<FileInfoWrapper>> allDistinctItemsToProcess;

        private readonly Lazy<IList<IEnumerable<FileInfoWrapper>>> fileDuplicatesSorted;

        public FileProcessor(
            Func<string, FileInfoWrapper> fileInfoWrapperFactory,
            IProcessorData processorData)
        {
            this.allDistinctItemsToProcess = new Lazy<List<FileInfoWrapper>>(
                () => AllDistinctItemsToProcessFactory(fileInfoWrapperFactory, processorData),
                isThreadSafe: true);

            this.fileDuplicatesSorted = new Lazy<IList<IEnumerable<FileInfoWrapper>>>(
                () => FileDuplicatesSortedFactory(this.GetAllDistinctItemsToProcess()),
                isThreadSafe: true);
        }

        public IList<FileInfoWrapper> GetAllDistinctItemsToProcess()
        {
            return this.allDistinctItemsToProcess.Value;
        }

        public IList<IEnumerable<FileInfoWrapper>> GetFileDuplicatesSorted()
        {
            return this.fileDuplicatesSorted.Value;
        }

        private static List<FileInfoWrapper> AllDistinctItemsToProcessFactory(Func<string, FileInfoWrapper> fileInfoWrapperFactory, IProcessorData processorData)
        {
            var patternStr = processorData.FileMask;
            var pattern = new WildcardPattern(patternStr, WildcardOptions.IgnoreCase);
            var searchOption = processorData.SearchOption;

            var filesFromInputDirectories = processorData.InputItems
                .Where(path => Directory.Exists(path))
                .SelectMany(path => Directory.EnumerateFiles(path, patternStr, searchOption));

            var inputFiles = processorData.InputItems
                .Where(path => File.Exists(path))
                .Where(path => pattern.IsMatch(Path.GetFileName(path)));

            return filesFromInputDirectories.Concat(inputFiles)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(fullFileName => fileInfoWrapperFactory(fullFileName))
                .ToList();
        }

        private static IList<IEnumerable<FileInfoWrapper>> FileDuplicatesSortedFactory(IEnumerable<FileInfoWrapper> distinctFileInfoWrappers)
        {
            return distinctFileInfoWrappers
                .GroupBy(item => item, FileInfoWrapperComparer.LengthComparer)
                .Where(MoreThanOneElementInGroup)
                .SelectMany(group => group)
                .AsParallel()
                .GroupBy(item => item, FileInfoWrapperComparer.HashComparer)
                .Where(MoreThanOneElementInGroup)
                .OrderByDescending(item => item.Key.FileInfo.Length)
                .Select(item => item.Select(subItem => subItem))
                .ToList();
        }

        private static bool MoreThanOneElementInGroup<T, V>(IGrouping<T, V> group)
        {
            return group.Skip(1).Any();
        }
    }
}
