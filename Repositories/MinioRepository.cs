using System.Reactive.Linq;

using Microsoft.OpenApi.Extensions;

using Minio;

using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IMinioRepository
    {
        Task<ICollection<string>?> GetBucketCollectionAsync();
        Task<bool> AddBucketAsync(AvailableBucket bucketName);
        Task<bool> RemoveBucketAsync(AvailableBucket bucketName);
        Task<string?> GetObjectAsync(AvailableBucket bucketName, Guid objectId, string fileExt);
        Task<ICollection<string>?> GetObjectCollectionAsync(AvailableBucket bucketName);
        Task<bool> AddObjectAsync(AvailableBucket bucketName, Guid objectId, IFormFile file);
        Task<bool> AddObjectCollectionAsync(AvailableBucket bucketName, ICollection<Guid> objectIdCollection, IFormFileCollection fileCollection);
        Task<bool> RemoveObjectAsync(AvailableBucket bucketName, Guid objectId);
    }

    public class MinioRepository : IMinioRepository
    {
        private readonly MinioClient _minioClient;

        public MinioRepository(MinioClient minioClient)
        {
            _minioClient = minioClient.Build();
        }

        public async Task<bool> AddBucketAsync(AvailableBucket bucket)
        {
            string bucketName = await IsBucketExistedAsync(bucket);

            if (!string.IsNullOrEmpty(bucketName))
            {
                throw new Exception($"Bucket {bucketName} already exists");
            }

            try
            {
                MakeBucketArgs mbArgs = new MakeBucketArgs().WithBucket(bucket: bucketName);
                await _minioClient.MakeBucketAsync(args: mbArgs);
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> AddObjectAsync(AvailableBucket bucket, Guid objectId, IFormFile file)
        {
            string bucketName = await IsBucketExistedAsync(bucket);

            if (string.IsNullOrEmpty(bucketName))
            {
                _ = await AddBucketAsync(bucket);
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
                // await fileStream.DisposeAsync();
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> AddObjectCollectionAsync(AvailableBucket bucket, ICollection<Guid> objectIdCollection, IFormFileCollection fileCollection)
        {
            for (int index = 0; index < objectIdCollection.Count; index++)
            {
                bool success = await AddObjectAsync(
                    bucket,
                    objectId: objectIdCollection.ElementAt(index),
                    file: fileCollection.ElementAt(index));
                if (!success) { return false; }
            }
            return true;
        }

        public async Task<ICollection<string>?> GetBucketCollectionAsync()
        {
            Minio.DataModel.ListAllMyBucketsResult result = await _minioClient.ListBucketsAsync();

            return result.Buckets.AsReadOnly().Select(selector: item => item.Name).ToList();
        }

        public async Task<string?> GetObjectAsync(AvailableBucket bucket, Guid objectId, string fileExt)
        {
            bool objectExisted = await IsObjectExistedAsync(bucket, objectId);

            if (!objectExisted) { return null; }

            try
            {
                PresignedGetObjectArgs pgoArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucket: GetBucketName(bucket))
                    .WithObject(obj: objectId.ToString() + "." + fileExt)
                    .WithExpiry(expiry: 60 * 5);
                return await _minioClient.PresignedGetObjectAsync(args: pgoArgs);
            }
            catch { return null; }
        }

        public async Task<ICollection<string>?> GetObjectCollectionAsync(AvailableBucket bucket)
        {
            string bucketName = await IsBucketExistedAsync(bucket);

            if (string.IsNullOrEmpty(bucketName))
            {
                return null;
            }

            try
            {
                ListObjectsArgs loArgs = new ListObjectsArgs()
                    .WithBucket(bucket: bucketName);
                // var collection = await _minioClient.ListObjectsAsync(args: loArgs).Select(selector: item => item.Key);
                return default;
            }
            catch { return null; }
        }

        public async Task<bool> RemoveBucketAsync(AvailableBucket bucket)
        {
            string bucketName = await IsBucketExistedAsync(bucket);

            if (string.IsNullOrEmpty(bucketName))
            {
                throw new Exception($"Bucket {bucketName} does not exist");
            }

            try
            {
                RemoveBucketArgs rbArgs = new RemoveBucketArgs().WithBucket(bucket: bucketName);
                await _minioClient.RemoveBucketAsync(args: rbArgs);
            }
            catch { return false; }
            return true;
        }

        public Task<bool> RemoveObjectAsync(AvailableBucket bucket, Guid objectId)
        {
            _ = GetBucketName(bucket);
            throw new NotImplementedException();
        }

        private async Task<bool> IsObjectExistedAsync(AvailableBucket bucket, Guid objectId)
        {
            string bucketName = await IsBucketExistedAsync(bucket);

            if (string.IsNullOrEmpty(bucketName))
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

        private async Task<string> IsBucketExistedAsync(AvailableBucket bucket)
        {
            string bucketName = GetBucketName(bucket);
            try
            {
                BucketExistsArgs beArgs = new BucketExistsArgs().WithBucket(bucket: bucketName);
                bool existedBucket = await _minioClient.BucketExistsAsync(args: beArgs);

                return bucketName;
            }
            catch { return string.Empty; }
        }

        private static string GetBucketName(AvailableBucket bucket)
        {
            return bucket.GetDisplayName();
        }
    }
}