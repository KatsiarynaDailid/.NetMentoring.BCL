using FileSystemDistributor.Configuration;
using FileSystemDistributor.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using Strings = FileSystemDistributor.Resources.Strings;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

namespace FileSystemDistributor
{
    public class FileSystemDistributor
    {
        private List<RuleElement> rules;
        private DirectoryInfo defaultDir;
        private ILogger log;
        private const int FileCheckTimoutMiliseconds = 2000;

        public FileSystemDistributor(List<RuleElement> rules, DirectoryInfo defaultDir, ILogger log)
        {
            this.rules = rules;
            this.defaultDir = defaultDir;
            this.log = log;
        }

        public void MoveFile(string name, string sourcePath)
        {
            int matchCount = 0;
            foreach (var rule in rules)
            {
                var template = new Regex(rule.Template);
                var isMatch = template.IsMatch(name);
                if (isMatch)
                {
                    matchCount++;
                    log.Log(String.Format(Strings.RuleFound, name));
                    string destinationPath = CreateDestinationPath(rule, name, matchCount);
                    Move(sourcePath, destinationPath);
                    log.Log(String.Format(Strings.FileMoved, name, destinationPath));
                    return;
                }
            }

            string defaultDestinationPath = Path.Combine(defaultDir.FullName, name);
            log.Log(String.Format(Strings.RuleNotFound, name));
            Move(sourcePath, defaultDestinationPath);
            log.Log(String.Format(Strings.FileMoved, name, defaultDestinationPath));
        }

        private void Move(string sourcePath, string destinationPath)
        {
            // Create directory if it is not exists yet
            string dir = Path.GetDirectoryName(destinationPath);
            Directory.CreateDirectory(dir);
            var ableToAccess = false;
            log.Log(String.Format(Strings.FileMoveStart, sourcePath, destinationPath));
            do
            {
                try
                {
                    if (File.Exists(destinationPath))
                    {
                        log.Log(String.Format(Strings.FileAlreadyExists, destinationPath));
                        File.Delete(destinationPath);
                        log.Log(String.Format(Strings.FileDeleted, destinationPath));
                        break;
                    }
                    File.Move(sourcePath, destinationPath);
                    ableToAccess = true;
                }
                catch (FileNotFoundException)
                {
                    log.Log(String.Format(Strings.FileNotFound, sourcePath));
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(FileCheckTimoutMiliseconds);
                }
            } while (!ableToAccess);
        }

        private string CreateDestinationPath(RuleElement rule, string name, int matchCount)
        {
            string ext = Path.GetExtension(name);
            string fileName = Path.GetFileNameWithoutExtension(name);
            var result = new StringBuilder().Append(Path.Combine(rule.DestinationDirectory, fileName));
            if (rule.IsDateNeeded)
            {
                var format = CultureInfo.CurrentCulture.DateTimeFormat;
                format.DateSeparator = ".";
                result.Append($"_{DateTime.Now.ToLocalTime().ToString(format.ShortDatePattern)}");
            }
            if (rule.IsSerialNumberNeeded)
            {
                result.Append($"_{matchCount}");
            }

            result.Append(ext);
            return result.ToString();
        }
    }
}
