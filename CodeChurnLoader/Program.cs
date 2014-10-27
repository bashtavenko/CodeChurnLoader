using System;
using System.Configuration;

using CodeChurnLoader.Data;
using CodeChurnLoader.Data.Github;

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

            var logger = new Logger();
            LoaderContext context = null;
            try
            {
                context = new LoaderContext();
                RepoCredentials repoCredentials = ConfigurationManager.GetSection("RepoCredentials") as RepoCredentials;
                if (repoCredentials == null)
                {
                    throw new ApplicationException("RepoCredential configuration section is missing");
                }
                var provider = new GithubProvider(repoCredentials);
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
    }
}
