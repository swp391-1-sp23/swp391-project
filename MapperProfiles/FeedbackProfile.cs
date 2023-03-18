using AutoMapper;

using SWP391.Project.Entities;

namespace SWP391.Project.MapperProfiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<FeedbackEntity, FeedbackSimplified>();
        }
    }
}