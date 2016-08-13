using System.IO;

namespace FileComparer
{
    public interface IProcessorData
    {
        string[] InputItems { get; }

        string FileMask { get; }

        SearchOption SearchOption { get; }
    }
}