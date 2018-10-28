using System.Configuration;
using System.Text.RegularExpressions;

namespace FileSystemDistributor.Configuration
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("template", IsKey = true)]
        public string Template
        {
            get { return (string)base["template"]; }
        }

        [ConfigurationProperty("destinationDir")]
        public string DestinationDirectory
        {
            get { return (string)base["destinationDir"]; }
        }

        [ConfigurationProperty("isSerialNumberNeeded")]
        public bool IsSerialNumberNeeded
        {
            get { return (bool)base["isSerialNumberNeeded"]; }
        }

        [ConfigurationProperty("isDateNeeded")]
        public bool IsDateNeeded
        {
            get { return (bool)base["isDateNeeded"]; }
        }
    }
}
