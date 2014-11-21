using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using AutoMapper;

using CodeChurnLoader.Data;

namespace CodeChurnLoader
{
    public class Loader
    {
        private readonly LoaderContext _context;
        private readonly ILogger _logger;

        public Loader(LoaderContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            AutoMapperConfig.CreateMaps();
        }                

        public void Load(IGitProvider provider, string repoName, DateTime from, DateTime to)
        {
            _logger.Log(string.Format("Getting {0} commits from {1:s} to {2:s} ...", repoName, from, to));
            List<Commit> commits = provider.GetCommits(repoName, from, to);
            if (commits.Any())
            {
                SaveCommits(from, repoName, commits);                
            }
            else
            {
                _logger.Log("No commits for this date range.");
            }
        }

        /// <summary>
        /// Saves commits for the given rep
        /// </summary>
        /// <param name="repoName">Name of the repository</param>
        /// <param name="commits">Commit list</param>
        public void SaveCommits(DateTime runDate, string repoName, List<Commit> commits)
        {
            if (HaveDataForThisDate(runDate))
            {
                _logger.Log("Already have data for this date range.");
                return;
            }

            _logger.Log("Saving to database...");

            var dimDate = new DimDate(runDate);
            dimDate = GetOrAddEntity<DimDate>(_context.Dates, dimDate, delegate(DimDate d) { return d.Date == dimDate.Date; });            
            
            var repo = new DimRepo { Name = repoName };
            repo = GetOrAddEntity<DimRepo>(_context.Repos, repo, delegate(DimRepo r) { return r.Name == repo.Name; });
            FactCodeChurn churn;
            foreach (var commit in commits)
            {
                DimCommit dimCommit = Mapper.Map<DimCommit>(commit);
                dimCommit.Repo = repo;
                _context.Commits.Add(dimCommit);
                churn = Mapper.Map<FactCodeChurn>(commit);
                churn.Commit = dimCommit;
                churn.Date = dimDate;
                _context.Churn.Add(churn);

                foreach (var file in commit.Files)
                {
                    DimFile dimFile = Mapper.Map<DimFile>(file);
                    dimFile.Commit = dimCommit;
                    _context.Files.Add(dimFile);
                    churn = Mapper.Map<FactCodeChurn>(file);
                    churn.File = dimFile;
                    churn.Date = dimDate;
                    _context.Churn.Add(churn);
                }

                _context.SaveChanges();            
            }
            _logger.Log("Done.");
        }

        /// <summary>
        /// Gets existing entity from db or adds one
        /// </summary>
        /// <typeparam name="T">Dimension (type, module, namespace, etc)</typeparam>
        /// <param name="list">List of these entities from the context</param>
        /// <param name="src">Entity itself</param>
        /// <param name="where">Where clause used to search for this entity</param>
        /// <returns>Newly added entity of entity from database</returns>
        private T GetOrAddEntity<T>(DbSet<T> list, T src, Func<T, bool> where) where T : class
        {
            IEnumerable<T> srcFromDb = list
                .Where(where)
                .Take(2);

            if (srcFromDb != null && srcFromDb.Count() == 1)
            {
                return srcFromDb.First();
            }
            else
            {
                list.Add(src);
                _context.SaveChanges();
                return src;
            }
        }

        private bool HaveDataForThisDate (DateTime date)
        {
            return _context.Churn.Where(c => c.Date.Date == date.Date).Any();
        }
    }
}
