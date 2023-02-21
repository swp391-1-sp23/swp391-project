using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using swp391-project.Project.DbContexts;
using swp391-project.Project.Entities;

namespace swp391-project.Repositories
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