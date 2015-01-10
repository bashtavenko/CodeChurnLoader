using System;
using System.Configuration;
using System.Linq;

using CodeChurnLoader.Data;
using CodeChurnLoader.Data.Github;
using CodeChurnLoader.Data.Bitbucket;

namespace CodeChurnLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            LoaderConfiguration config = new LoaderConfiguration();
            if (!CommandLine.Parser.Default.ParseArguments(args, config))
            {
                return;
            }
            config.SetupDates();
            
            var logger = new Logger();
            LoaderContext context = null;
            try
            {
                context = new LoaderContext();
                IGitProvider provider = GetProvider(config.ProviderType);
                var loader = new Loader(context, logger);
                loader.Load(provider, config.Repo, config.From, config.To);
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
            finally
            {
                if (context !=null)
                {
                    context.Dispose();
                }
            }
        }

        private static IGitProvider GetProvider (ProviderType providerType)
        {
            var loaderConfigurationSection = (ConfigurationManager.GetSection("LoaderConfiguration") as LoaderConfigurationSection);
            if (loaderConfigurationSection == null)
            {
                throw new ApplicationException("LoaderConfiguration section is missing");
            }
            var providerConfig = loaderConfigurationSection.Providers.Where(w => w.Type == providerType).FirstOrDefault();
            if (providerConfig == null)
            {
                throw new ApplicationException(string.Format ("{0} provider configuration is missing", providerType));
            }
            IGitProvider provider;
            if (providerType == ProviderType.Github)
            {
                var githubService = new GithubService(providerConfig);
                provider = new GithubProvider(githubService);
            }
            else
            {
                var bitbucketService = new BitbucketService(providerConfig);
                provider = new BitbucketProvider (bitbucketService);
            }

            return provider;
        }
    }
}
