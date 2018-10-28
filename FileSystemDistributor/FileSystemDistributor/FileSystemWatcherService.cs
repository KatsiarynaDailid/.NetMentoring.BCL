using FileSystemDistributor.Configuration;
using FileSystemDistributor.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using Strings = FileSystemDistributor.Resources.Strings;
using FileSystemDistributor.EventArgs;

namespace FileSystemDistributor
{
    public class FileSystemWatcherService
    {
        private List<FileSystemWatcher> fileSystemWatchers;
        private List<DirectoryElement> directories;
        private ILogger log;

        public event EventHandler<FileCreatedEventArgs> FileCreated;

        public FileSystemWatcherService(List<DirectoryElement> directories, ILogger log)
        {
            this.directories = directories;
            this.log = log;
            FillListOfWatchers();
        }

        private void OnCreated(string name, string path)
        {
            var temp = FileCreated;
            if (temp != null)
            {
                FileCreated(this, new FileCreatedEventArgs { Name = name, FilePath = path });
            }
        }

        private void FillListOfWatchers()
        {
            foreach (var dir in directories)
            {
                fileSystemWatchers = new List<FileSystemWatcher>(); 
                FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(dir.SourceDirectory);
                fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
                fileSystemWatcher.EnableRaisingEvents = true;

                fileSystemWatcher.Created += (s, args) =>
                {
                    log.Log(string.Format(Strings.FileFound, args.Name, File.GetCreationTime(args.FullPath)));
                    OnCreated(args.Name, args.FullPath);
                };

                fileSystemWatchers.Add(fileSystemWatcher);
            }
        }
    }
}
