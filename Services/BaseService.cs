using AutoMapper;

namespace SWP391.Project.Services
{
    public class BaseService
    {
        protected IMapper Mapper { get; }
        public BaseService(IMapper mapper)
        {
            Mapper = mapper;
        }
    }
}