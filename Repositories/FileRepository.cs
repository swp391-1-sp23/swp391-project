using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IFileRepository : IBaseRepository<FileEntity> { }

    public class FileRepository : BaseRepository<FileEntity>, IFileRepository
    {
        public FileRepository(ProjectDbContext dbContext) : base(dbContext) { }
    }
}