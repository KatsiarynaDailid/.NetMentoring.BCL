using FileSystemDistributor.Utils.Interfaces;
using System;

namespace FileSystemDistributor.Utils
{
    public class Logger : ILogger
    {
        private const string sectionSeparatorStart = "************************ Exception Information ****************************";
        private const string sectionSeparatorEnd = "***************************************************************************";

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogException(Exception ex)
        {
            LogException(ex, "");
        }
        public void LogException(Exception ex, string message = "")
        {
            Console.WriteLine(sectionSeparatorStart);
            Console.WriteLine(ex);
            if (ex.Data.Keys.Count > 0)
            {
                Console.WriteLine("Exception Data:");
                foreach (var key in ex.Data.Keys)
                {
                    Console.WriteLine(key + ": " + ex.Data[key].ToString());
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine($"Message: {message}");
            }
            Console.WriteLine(sectionSeparatorEnd);
        }
    }
}
