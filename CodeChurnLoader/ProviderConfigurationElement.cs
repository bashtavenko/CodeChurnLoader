﻿using System.Configuration;

namespace CodeChurnLoader
{
    public class ProviderConfigurationElement : ConfigurationElement
    {
        const string _typeProperty = "Type";
        const string _ownerProperty = "Owner";
        const string _userNameProperty = "UserName";
        const string _passwordProperty = "Password";

        [ConfigurationProperty(_typeProperty, IsRequired = true)]
        public ProviderType Type
        {
            get
            {
                return (ProviderType)this[_typeProperty];
            }
            set
            {
                this[_typeProperty] = value;
            }
        }

        [ConfigurationProperty(_ownerProperty, IsRequired = true)]
        public string Owner
        {
            get
            {
                return (string)this[_ownerProperty];
            }
            set
            {
                this[_ownerProperty] = value;
            }
        }

        [ConfigurationProperty(_userNameProperty, IsRequired = false)]
        public string UserName
        {
            get
            {
                return (string)this[_userNameProperty];
            }
            set
            {
                this[_userNameProperty] = value;
            }
        }
        

        [ConfigurationProperty(_passwordProperty, IsRequired = false)]
        public string Password
        {
            get
            {
                return (string)this[_passwordProperty];
            }
            set
            {
                this[_passwordProperty] = value;
            }
        }        
    }
}
