using System.Linq;

using AutoMapper;

namespace CodeChurnLoader
{
    public class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<CodeChurnLoader.Data.Github.File, CodeChurnLoader.Data.File>()
                .ForMember(m => m.NumberOfAdditions, opt => opt.MapFrom(src => src.Additions))
                .ForMember(m => m.NumberOfDeletions, opt => opt.MapFrom(src => src.Deletions))
                .ForMember(m => m.NumberOfChanges, opt => opt.MapFrom(src => src.Changes));

            Mapper.CreateMap<CodeChurnLoader.Data.Github.Commit, CodeChurnLoader.Data.Commit>()
                .ForMember(m => m.NumberOfAdditions, opt => opt.MapFrom(src => src.Stats.Additions))
                .ForMember(m => m.NumberOfChanges, opt => opt.MapFrom(src => src.Files.Sum(s => s.Changes)))
                .ForMember(m => m.NumberOfDeletions, opt => opt.MapFrom(src => src.Stats.Deletions))
                .ForMember(m => m.Message, opt => opt.MapFrom(src => src.CommitProperties.Message))
                .ForMember(m => m.Committer, opt => opt.MapFrom(src => src.CommitProperties.Committer.Name ?? src.CommitProperties.Committer.Login))
                .ForMember(m => m.Date, opt => opt.MapFrom(src => src.CommitProperties.Committer.Date));

            Mapper.CreateMap<CodeChurnLoader.Data.Commit, CodeChurnLoader.Data.DimCommit>()
                .ForMember(m => m.Files, opt => opt.Ignore());

            Mapper.CreateMap<CodeChurnLoader.Data.File, CodeChurnLoader.Data.DimFile>();

            Mapper.CreateMap<CodeChurnLoader.Data.Commit, CodeChurnLoader.Data.FactCodeChurn>()
                .ForMember(m => m.Date, opt => opt.Ignore())                
                .ForMember(m => m.LinesAdded, opt => opt.MapFrom(src => src.NumberOfAdditions))
                .ForMember(m => m.LinesModified, opt => opt.MapFrom(src => src.NumberOfChanges))
                .ForMember(m => m.LinesDeleted, opt => opt.MapFrom(src => src.NumberOfDeletions))
                .ForMember(m => m.TotalChurn, opt => opt.MapFrom(src => src.TotalChurn));

            Mapper.CreateMap<CodeChurnLoader.Data.File, CodeChurnLoader.Data.FactCodeChurn>()
                .ForMember(m => m.LinesAdded, opt => opt.MapFrom(src => src.NumberOfAdditions))
                .ForMember(m => m.LinesModified, opt => opt.MapFrom(src => src.NumberOfChanges))
                .ForMember(m => m.LinesDeleted, opt => opt.MapFrom(src => src.NumberOfDeletions))
                .ForMember(m => m.TotalChurn, opt => opt.MapFrom(src => src.NumberOfAdditions + src.NumberOfChanges + src.NumberOfDeletions));
        }
    }
}
