using System;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
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
                .ForMember(m => m.NumberOfDeletions, opt => opt.MapFrom(src => src.Stats.Deletions));                
        }
    }
}
