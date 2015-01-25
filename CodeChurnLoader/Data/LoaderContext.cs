using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CodeChurnLoader.Data
{
    public class LoaderContext : DbContext
    {
        public LoaderContext(string databaseName, IDatabaseInitializer<LoaderContext> initializer)
            : base(databaseName)
        {
            Database.SetInitializer(initializer);
        }

        public LoaderContext(string connectionString)
            : base(nameOrConnectionString: connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LoaderContext>());
        }

        public LoaderContext() : base("CodeChurnLoaderWarehouse")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LoaderContext>());                        
        }

        public DbSet<DimRepo> Repos { get; set; }
        public DbSet<DimCommit> Commits { get; set; }
        public DbSet<DimFile> Files { get; set; }
        public DbSet<DimDate> Dates { get; set; }
        public DbSet<FactCodeChurn> Churn { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<DimRepo>().HasKey(k => k.RepoId);
            modelBuilder.Entity<DimRepo>().Property(k => k.Name).HasColumnType("varchar").HasMaxLength(255);
            modelBuilder.Entity<DimRepo>().Property(k => k.Name).IsRequired();

            modelBuilder.Entity<DimCommit>().HasKey(k => k.CommitId);
            modelBuilder.Entity<DimCommit>().Property(k => k.Sha).HasColumnType("varchar").HasMaxLength(255);
            modelBuilder.Entity<DimCommit>().Property(k => k.Sha).IsRequired();
            modelBuilder.Entity<DimCommit>().Property(k => k.Url).IsRequired();
            modelBuilder.Entity<DimCommit>().Property(k => k.Committer).IsRequired();
            modelBuilder.Entity<DimCommit>().Property(k => k.Committer).HasColumnType("varchar").HasMaxLength(255);
            modelBuilder.Entity<DimCommit>().Property(k => k.Message);
            modelBuilder.Entity<DimCommit>().HasMany(c => c.Files)
                .WithMany(f => f.Commits)
                .Map(c =>
                {
                    c.ToTable("DimCommitFile");
                    c.MapLeftKey("CommitId");
                    c.MapRightKey("FileId");
                });
            
            modelBuilder.Entity<DimFile>().HasKey(k => k.FileId);
            modelBuilder.Entity<DimFile>().Property(k => k.FileName).HasColumnType("varchar").HasMaxLength(255);
            modelBuilder.Entity<DimFile>().Property(k => k.FileName).IsRequired();
            modelBuilder.Entity<DimFile>().Property(k => k.FileExtension).HasColumnType("varchar").HasMaxLength(255);
            modelBuilder.Entity<DimFile>().Property(k => k.FileExtension).IsRequired();
            modelBuilder.Entity<DimFile>().Property(k => k.Url).IsOptional(); // Bitbucket doesn't provide file urls
            
                        
            modelBuilder.Entity<DimDate>().HasKey(k => k.DateId);
            
            modelBuilder.Entity<FactCodeChurn>().HasKey(k => k.CodeChurnId);            
        }
    }
}
