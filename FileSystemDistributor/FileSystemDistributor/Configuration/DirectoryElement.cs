using System.Configuration;
using System.IO;

namespace FileSystemDistributor.Configuration
{
    public class DirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true)]
        public string SourceDirectory
        {
            get
            {
                return (string)base["path"];
            }
        }
    }
}
