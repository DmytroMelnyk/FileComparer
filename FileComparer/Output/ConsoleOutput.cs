using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables.Core;
using FileComparer.Utils;

namespace FileComparer.Output
{
    public class ConsoleOutput : IOutput
    {
        public void PrintTable(IList<FileInfoWrapper> tableData, int resultLimit)
        {
            var table = new ConsoleTable("#", "Full file name");
            resultLimit = Math.Min(tableData.Count, resultLimit);
            for (int i = 0; i < resultLimit; i++)
            {
                table.AddRow(i, tableData[i].ToString());
            }

            table.ToBeContinued(tableData.Count, resultLimit);
            table.Write(Format.Alternative);
        }

        public void PrintTable(IList<IEnumerable<FileInfoWrapper>> tableData, int resultLimit)
        {
            var table = new ConsoleTable("#", "Full file name");
            resultLimit = Math.Min(tableData.Count, resultLimit);
            for (int i = 0; i < resultLimit; i++)
            {
                table.AddRow(i, tableData[i].First());
                foreach (var item in tableData[i].Skip(1))
                {
                    table.AddRow(string.Empty, item);
                }
            }

            table.ToBeContinued(tableData.Count, resultLimit);
            table.Write(Format.Alternative);
        }

        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }
    }
}
