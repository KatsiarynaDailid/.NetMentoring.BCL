using System;

namespace FileSystemDistributor.Utils.Interfaces
{
    public interface ILogger
    {
        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="message">Message to log</param>
        void Log(string message);

        /// <summary>
        /// Log exception details
        /// </summary>
        /// <param name="ex">Exception to log</param>
        void LogException(Exception ex);

        /// <summary>
        /// Log exception details and the given message.
        /// </summary>
        /// <param name="ex">Exception to log</param>
        /// <param name="message">Message about the exception</param>
        void LogException(Exception ex, string message);
    }
}
