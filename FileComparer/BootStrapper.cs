using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileComparer
{
    public class BootStrapper
    {
        private readonly Func<IProcessorData, FileProcessor> fileProcessorFactory;

        private readonly IOutput output;

        public BootStrapper(
            Func<IProcessorData, FileProcessor> fileProcessorFactory,
            IOutput output)
        {
            this.fileProcessorFactory = fileProcessorFactory;
            this.output = output;
        }

        public void Run(CommandLineOptions options)
        {
            var fileProcessor = this.fileProcessorFactory(options);

            var allItems = fileProcessor.GetAllDistinctItemsToProcess();
            if (allItems.Skip(1).Any())
            {
                this.output.WriteLine("All items to process.");
                this.output.PrintTable(allItems, options.ResultLimit);
            }
            else
            {
                this.output.WriteLine("Should be more than 2 files to continue.");
                return;
            }

            var duplicates = this.DoLongRunningTask(fileProcessor.GetFileDuplicatesSorted);
            if (duplicates.Any())
            {
                this.output.WriteLine("Group(s) of equal files.");
                this.output.PrintTable(duplicates, options.ResultLimit);
            }
            else
            {
                this.output.WriteLine("All files are distinct.");
            }

            this.output.WriteLine("Done!");
        }

        private T DoLongRunningTask<T>(Func<T> func)
        {
            this.output.WriteLine("Working...");
            var task = Task.Run(func);
            while (!task.IsCompleted)
            {
                this.output.Write(".");
                Thread.Sleep(1000);
            }

            this.output.WriteLine(string.Empty);
            return task.Result;
        }
    }
}