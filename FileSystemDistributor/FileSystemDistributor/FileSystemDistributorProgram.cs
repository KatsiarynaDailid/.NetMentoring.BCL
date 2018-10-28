using System;
using System.Collections.Generic;
using FileSystemDistributorSettings = FileSystemDistributor.Configuration.FileSystemDistributorSettings;
using RuleElement = FileSystemDistributor.Configuration.RuleElement;
using DirectoryElement = FileSystemDistributor.Configuration.DirectoryElement;
using Strings = FileSystemDistributor.Resources.Strings;
using System.Configuration;
using FileSystemDistributor.Utils.Interfaces;
using FileSystemDistributor.Utils;
using FileSystemDistributor.EventArgs;
using System.Threading;
using System.Globalization;
using System.IO;

namespace FileSystemDistributor
{
    class FileSystemDistributorProgram
    {
        private static List<DirectoryElement> directories;
        private static List<RuleElement> rules;
        private static FileSystemDistributor distributor;
        static void Main(string[] args)
        {
            ILogger log = new Logger();
            FileSystemDistributorSettings config = (FileSystemDistributorSettings)ConfigurationManager.GetSection("fileSystemDistributorSettings");

            if (config != null)
            {
                ReadConfig(config);
            }
            else
            {
                log.Log(Strings.ConfigNotFound);
                return;
            }

            log.Log(Strings.CurrentCulture);

            distributor = new FileSystemDistributor(rules, new DirectoryInfo(config.Rules.DefaultDirectory), log);
            var watcher = new FileSystemWatcherService(directories, log);

            watcher.FileCreated += OnFileCreated;

            log.Log(Strings.ExitMessage);
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        private static void OnFileCreated(object sender, FileCreatedEventArgs args)
        {
           distributor.MoveFile(args.Name, args.FilePath);
        }

        private static void ReadConfig(FileSystemDistributorSettings config)
        {
            directories = new List<DirectoryElement>(config.Directories.Count);
            rules = new List<RuleElement>();

            foreach (DirectoryElement directory in config.Directories)
            {
                directories.Add(directory);
            }

            foreach (RuleElement rule in config.Rules)
            {
                rules.Add(rule);
            }

            CultureInfo.DefaultThreadCurrentCulture = config.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = config.Culture;
            CultureInfo.CurrentUICulture = config.Culture;
            CultureInfo.CurrentCulture = config.Culture;
        }
    }
}
