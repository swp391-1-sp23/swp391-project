using AutoMapper.Configuration.Annotations;

using SWP391.Project.Entities;

namespace SWP391.Project.Models.Dtos.Brand
{
    public class BrandDto : BrandSimplified
    {
        [Ignore]
        public (FileSimplified LogoInfo, string? LogoUrl) Logo { get; set; }
    }
}