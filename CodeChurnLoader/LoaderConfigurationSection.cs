using System.Configuration;

namespace CodeChurnLoader
{
    public class LoaderConfigurationSection : ConfigurationSection
    {
        const string _providersProperty = "Providers";
                
        [ConfigurationProperty(_providersProperty, IsRequired = true)]
        [ConfigurationCollection(typeof(ProviderConfigurationElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ProviderCollection Providers
        {
            get
            {
                return (ProviderCollection) this[_providersProperty];
            }            
        }        
    }
}
