using AutoMapper;

using SWP391.Project.Entities;

namespace SWP391.Project.MapperProfiles
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            _ = CreateMap<FileEntity, FileSimplified>();
        }
    }
}