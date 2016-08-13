using System;
using Autofac;
using CommandLine;
using FileComparer.Output;

namespace FileComparer
{
    internal class Program
    {
        private static Lazy<IContainer> containerBuilder = new Lazy<IContainer>(() =>
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MD5FileHashFactoryProvider>().As<IFileHashFactoryProvider>().SingleInstance();
            builder.RegisterType<ConsoleOutput>().As<IOutput>().SingleInstance();
            builder.RegisterType<FileInfoWrapper>().AsSelf();
            builder.RegisterType<FileProcessor>().AsSelf();
            builder.RegisterType<BootStrapper>().AsSelf();
            return builder.Build();
        });

        private static IContainer Container
        {
            get { return containerBuilder.Value; }
        }

        private static int Main(string[] args)
        {
            try
            {
                var options = new CommandLineOptions();
                if (Parser.Default.ParseArguments(args, options))
                {
                    using (var scope = Container.BeginLifetimeScope())
                    {
                        scope.Resolve<BootStrapper>().Run(options);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return 1;
            }

            Console.ReadKey();
            return 0;
        }
    }
}
