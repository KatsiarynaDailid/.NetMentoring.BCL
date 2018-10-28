using System.Configuration;
using System.Globalization;
using System.IO;

namespace FileSystemDistributor.Configuration
{
    public class FileSystemDistributorSettings : ConfigurationSection
    {
        [ConfigurationProperty("culture", DefaultValue = "en-US")]
        public CultureInfo Culture
        {
            get
            {
                return (CultureInfo)this["culture"];
            }
        }

        [ConfigurationProperty("directories")]
        [ConfigurationCollection(typeof(DirectoryElement), AddItemName = "directory")]
        public DirectoryElementCollection Directories
        {
            get { return (DirectoryElementCollection)this["directories"]; }
        }

        [ConfigurationCollection(typeof(RuleElement), AddItemName = "rule")]
        [ConfigurationProperty("rules")]
        public RuleElementCollection Rules
        {
            get { return (RuleElementCollection)this["rules"]; }
        }
    }
}
