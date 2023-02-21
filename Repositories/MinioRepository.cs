using System.Reactive.Linq;

using Minio;

namespace SWP391.Project.Repositories
{
    public interface IMinioRepository
    {
        Task<ICollection<string>?> GetBucketCollectionAsync();
        Task<bool> AddBucketAsync(string bucketName);
        Task<bool> RemoveBucketAsync(string bucketName);
        Task<string?> GetObjectAsync(string bucketName, Guid objectId);
        Task<ICollection<string>?> GetObjectCollectionAsync(string bucketName);
        Task<bool> AddObjectAsync(string bucketName, Guid objectId, IFormFile file);
        Task<bool> AddObjectCollectionAsync(string bucketName, ICollection<Guid> objectIdCollection, IFormFileCollection fileCollection);
        Task<bool> RemoveObjectAsync(string bucketName, Guid objectId);
    }

    public class MinioRepository : IMinioRepository
    {
        private readonly MinioClient _minioClient;

        public MinioRepository(MinioClient minioClient)
        {
            _minioClient = minioClient.Build();
        }

        public async Task<bool> AddBucketAsync(string bucketName)
        {
            bool bucketExisted = await IsBucketExistedAsync(bucketName);

            if (bucketExisted)
            {
                return true;
            }

            try
            {
                MakeBucketArgs mbArgs = new MakeBucketArgs().WithBucket(bucket: bucketName);
                await _minioClient.MakeBucketAsync(args: mbArgs);
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> AddObjectAsync(string bucketName, Guid objectId, IFormFile file)
        {
            bool bucketExisted = await IsBucketExistedAsync(bucketName);

            if (!bucketExisted)
            {
                _ = await AddBucketAsync(bucketName);
            }

            try
            {
                string fileExt = file.FileName.Split('.').ElementAt(index: -1);
                Stream fileStream = file.OpenReadStream();
                PutObjectArgs poArgs = new PutObjectArgs()
                    .WithBucket(bucket: bucketName)
                    .WithObject(obj: $"{objectId}.{fileExt}")
                    .WithStreamData(data: fileStream)
                    .WithObjectSize(size: fileStream.Length)
                    .WithContentType(type: file.ContentType);
                await _minioClient.PutObjectAsync(args: poArgs);
                await fileStream.DisposeAsync();
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> AddObjectCollectionAsync(string bucketName, ICollection<Guid> objectIdCollection, IFormFileCollection fileCollection)
        {
            bool bucketExisted = await IsBucketExistedAsync(bucketName);

            if (!bucketExisted)
            {
                _ = await AddBucketAsync(bucketName);
            }

            for (int index = 0; index < objectIdCollection.Count; index++)
            {
                bool success = await AddObjectAsync(
                    bucketName,
                    objectId: objectIdCollection.ElementAt(index),
                    file: fileCollection.ElementAt(index));
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<ICollection<string>?> GetBucketCollectionAsync()
        {
            Minio.DataModel.ListAllMyBucketsResult result = await _minioClient.ListBucketsAsync();

            return result.Buckets.AsReadOnly().Select(selector: item => item.Name).ToList();
        }

        public async Task<string?> GetObjectAsync(string bucketName, Guid objectId)
        {
            bool objectExisted = await IsObjectExistedAsync(bucketName, objectId);

            if (!objectExisted)
            {
                return null;
            }

            try
            {
                PresignedGetObjectArgs pgoArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucket: bucketName)
                    .WithObject(obj: objectId.ToString())
                    .WithExpiry(expiry: 60 * 5);
                return await _minioClient.PresignedGetObjectAsync(args: pgoArgs);
            }
            catch { return null; }
        }

        public async Task<ICollection<string>?> GetObjectCollectionAsync(string bucketName)
        {
            bool bucketExisted = await IsBucketExistedAsync(bucketName);

            if (!bucketExisted)
            {
                return null;
            }

            _ = new List<string>();
            try
            {
                ListObjectsArgs loArgs = new ListObjectsArgs()
                    .WithBucket(bucket: bucketName);
                // var collection = await _minioClient.ListObjectsAsync(args: loArgs).Select(selector: item => item.Key);
                return default;
            }
            catch { return null; }
        }

        public async Task<bool> RemoveBucketAsync(string bucketName)
        {
            bool bucketExisted = await IsBucketExistedAsync(bucketName);

            if (!bucketExisted)
            {
                return false;
            }

            try
            {
                RemoveBucketArgs rbArgs = new RemoveBucketArgs().WithBucket(bucket: bucketName);
                await _minioClient.RemoveBucketAsync(args: rbArgs);
            }
            catch { return false; }
            return true;
        }

        public Task<bool> RemoveObjectAsync(string bucketName, Guid objectId)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> IsObjectExistedAsync(string bucketName, Guid objectId)
        {
            bool bucketExisted = await IsBucketExistedAsync(bucketName);

            if (!bucketExisted)
            {
                return false;
            }

            try
            {
                StatObjectArgs soArgs = new StatObjectArgs()
                    .WithBucket(bucket: bucketName)
                    .WithObject(obj: objectId.ToString());
                Minio.DataModel.ObjectStat existingObject = await _minioClient.StatObjectAsync(args: soArgs);

                return existingObject == null;
            }
            catch { return false; }
        }

        private async Task<bool> IsBucketExistedAsync(string bucketName)
        {
            try
            {
                BucketExistsArgs beArgs = new BucketExistsArgs().WithBucket(bucket: bucketName);
                return await _minioClient.BucketExistsAsync(args: beArgs);
            }
            catch { return false; }
        }
    }
}