using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IImageRepository : IBaseRepository<ImageEntity>
{
}

public class ImageRepository : BaseRepository<ImageEntity>, IImageRepository
{
    public ImageRepository(ProjectDbContext dbContext) : base(dbContext)
    {
    }
}
}