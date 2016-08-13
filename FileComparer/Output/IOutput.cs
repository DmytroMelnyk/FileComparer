using System.Collections.Generic;

namespace FileComparer
{
    public interface IOutput
    {
        void WriteLine(string str);

        void PrintTable(IList<IEnumerable<FileInfoWrapper>> table, int resultLimit);

        void PrintTable(IList<FileInfoWrapper> table, int resultLimit);

        void Write(string str);
    }
}
