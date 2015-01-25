using System.Linq;
using System.Text.RegularExpressions;

using AutoMapper;

using CodeChurnLoader.Data.Bitbucket;

namespace CodeChurnLoader
{
    public class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<CodeChurnLoader.Data.Github.File, CodeChurnLoader.Data.File>();                

            Mapper.CreateMap<CodeChurnLoader.Data.Github.Commit, CodeChurnLoader.Data.Commit>()
                .ForMember(m => m.Additions, opt => opt.MapFrom(src => src.Stats.Additions))
                .ForMember(m => m.Changes, opt => opt.MapFrom(src => src.Stats.Changes))
                .ForMember(m => m.Deletions, opt => opt.MapFrom(src => src.Stats.Deletions))
                .ForMember(m => m.Message, opt => opt.MapFrom(src => src.CommitProperties.Message))
                .ForMember(m => m.Committer, opt => opt.MapFrom(src => src.CommitProperties.Committer.Name ?? src.CommitProperties.Committer.Login))
                .ForMember(m => m.Date, opt => opt.MapFrom(src => src.CommitProperties.Committer.Date));

            Mapper.CreateMap<CodeChurnLoader.Data.Commit, CodeChurnLoader.Data.DimCommit>()
                .ForMember(m => m.Files, opt => opt.Ignore());

            Mapper.CreateMap<CodeChurnLoader.Data.File, CodeChurnLoader.Data.DimFile>();            

            Mapper.CreateMap<CodeChurnLoader.Data.Commit, CodeChurnLoader.Data.FactCodeChurn>()
                .ForMember(m => m.Date, opt => opt.Ignore())
                .ForMember(m => m.LinesAdded, opt => opt.MapFrom(src => src.Additions))
                .ForMember(m => m.LinesDeleted, opt => opt.MapFrom(src => src.Deletions))                
                .ForMember(m => m.TotalChurn, opt => opt.MapFrom(src => src.Changes));                

            Mapper.CreateMap<CodeChurnLoader.Data.File, CodeChurnLoader.Data.FactCodeChurn>()
                .ForMember(m => m.LinesAdded, opt => opt.MapFrom(src => src.Additions))
                .ForMember(m => m.LinesDeleted, opt => opt.MapFrom(src => src.Deletions))
                .ForMember(m => m.TotalChurn, opt => opt.MapFrom(src => src.Changes));
                                    
            Mapper.CreateMap<CodeChurnLoader.Data.Bitbucket.Commit, CodeChurnLoader.Data.Commit>()
                .ForMember(m => m.Url, opt => opt.MapFrom(src => src.Urls.Html.Value))
                .ForMember(m => m.CommitterAvatarUrl, opt => opt.MapFrom(src => src.Author.User.Urls.Avatar.Value))
                .ForMember(m => m.Committer, opt => opt.ResolveUsing(new CommiterResolver()));

            Mapper.CreateMap<CodeChurnLoader.Data.Bitbucket.File, CodeChurnLoader.Data.File>();                
        }

        private class CommiterResolver : ValueResolver<Commit, string>
        {
            protected override string ResolveCore(Commit source)
            {
                if (source.Author.User != null)
                {
                    return source.Author.User.UserName;
                }
                else
                {
                    string rawName = source.Author.Raw;
                    var regex = new Regex(".*?(?= <)");
                    var match = regex.Match(source.Author.Raw);
                    return match.Success ? match.Value : source.Author.Raw;
                }
            }
        }
    }    
}
