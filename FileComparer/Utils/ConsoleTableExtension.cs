using System.Linq;
using ConsoleTables.Core;

namespace FileComparer.Utils
{
    internal static class ConsoleTableExtension
    {
        public static void ToBeContinued(this ConsoleTable @this, int dataCount, int dataLimit)
        {
            if (dataCount <= dataLimit)
            {
                return;
            }

            @this.AddRow(Enumerable.Repeat("...", @this.Columns.Count).ToArray());
        }
    }
}
