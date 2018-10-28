using System.Configuration;

namespace FileSystemDistributor.Configuration
{
    public class RuleElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("defaultDirectory")]
        public string DefaultDirectory
        {
            get { return (string)this["defaultDirectory"]; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement)element).Template;
        }
    }
}
