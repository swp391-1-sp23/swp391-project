using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public class BaseService
    {
        protected IMapper Mapper { get; }
        protected IFileRepository? FileRepository { get; }
        protected IMinioRepository? MinioRepository { get; }

        public BaseService(IMapper mapper)
        {
            Mapper = mapper;
        }

        public BaseService(IFileRepository fileRepository, IMinioRepository minioRepository, IMapper mapper)
        {
            Mapper = mapper;
            FileRepository = fileRepository;
            MinioRepository = minioRepository;
        }

        protected async Task<(FileSimplified FileInfo, string? FileUrl)?> GetFileAsync(AvailableBucket bucket, Guid fileId, string fileExtension)
        {
            FileEntity? file = await FileRepository!.GetByIdAsync(fileId);

            if (file == null)
            {
                return null;
            }

            (Guid ObjectId, string? ObjectUrl)? fileObject = await MinioRepository!.GetObjectAsync(bucketName: bucket, fileName: (objectId: fileId, fileExtension));

            return (Mapper.Map<FileSimplified>(file), fileObject?.ObjectUrl);
        }
    }
}