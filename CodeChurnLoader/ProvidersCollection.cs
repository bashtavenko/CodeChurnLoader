using System.Collections.Generic;
using System.Configuration;

namespace CodeChurnLoader
{    
    public class ProviderCollection : ConfigurationElementCollection, IEnumerable<ProviderConfigurationElement>
    {
        private List<ProviderConfigurationElement> _elements;

        public ProviderCollection()
        {
            _elements = new List<ProviderConfigurationElement>();
        }

        public ProviderConfigurationElement this[int index]
        {
            get { return (ProviderConfigurationElement)_elements[index]; }            
        }

        protected override ConfigurationElement CreateNewElement()
        {
            ProviderConfigurationElement newElement = new ProviderConfigurationElement();
            _elements.Add(newElement);
            return newElement;            
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return _elements.Find(e => e.Equals(element));            
        }

        public new IEnumerator<ProviderConfigurationElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
