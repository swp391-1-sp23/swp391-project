using System.Reactive.Linq;

using Microsoft.OpenApi.Extensions;

using Minio;

using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IMinioRepository
    {
        Task<ICollection<string>?> GetBucketCollectionAsync();
        // Task<bool> AddBucketAsync(AvailableBucket bucketName);
        // Task<bool> RemoveBucketAsync(AvailableBucket bucketName);
        Task<(Guid ObjectId, string? ObjectUrl)?> GetObjectAsync(AvailableBucket bucketName, (Guid objectId, string fileExtension) fileName);
        Task<ICollection<(Guid ObjectId, string ObjectUrl)>?> GetObjectCollectionAsync(AvailableBucket bucketName, ICollection<(Guid objectId, string fileExtension)> objectCollection);
        Task<bool> AddObjectAsync(AvailableBucket bucketName, IFormFile file, (Guid objectId, string fileExtension) fileName);
        Task<bool> AddObjectCollectionAsync(AvailableBucket bucketName, ICollection<(Guid objectId, string fileExtension)> objectCollection, IFormFileCollection fileCollection);
        Task<bool> RemoveObjectAsync(AvailableBucket bucketName, Guid objectId);
    }

    public class MinioRepository : IMinioRepository
    {
        private readonly MinioClient _minioClient;

        public MinioRepository(MinioClient minioClient)
        {
            _minioClient = minioClient.Build();
        }

        public async Task<bool> AddObjectAsync(AvailableBucket bucket, IFormFile file, (Guid objectId, string fileExtension) fileName)
        {
            string bucketName = await EnsureBucketExistedAsync(bucket);

            if (string.IsNullOrEmpty(bucketName))
            {
                return false;
            }

            try
            {
                (Guid objectId, string fileExtension) = fileName;

                Stream fileStream = file.OpenReadStream();

                PutObjectArgs poArgs = new PutObjectArgs()
                    .WithBucket(bucket: bucketName)
                    .WithObject(obj: $"{objectId}.{fileExtension}")
                    .WithStreamData(data: fileStream)
                    .WithObjectSize(size: fileStream.Length)
                    .WithContentType(type: file.ContentType);

                await _minioClient.PutObjectAsync(args: poArgs);
                // await fileStream.DisposeAsync();
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> AddObjectCollectionAsync(AvailableBucket bucket, ICollection<(Guid objectId, string fileExtension)> objectCollection, IFormFileCollection fileCollection)
        {
            for (int index = 0; index < objectCollection.Count; index++)
            {
                (Guid objectId, string fileExtension) fileName = objectCollection.ElementAt(index);

                bool success = await AddObjectAsync(
                    bucket,
                    file: fileCollection.ElementAt(index: index),
                    fileName);

                if (!success) { return false; }
            }
            return true;
        }

        public async Task<ICollection<string>?> GetBucketCollectionAsync()
        {
            Minio.DataModel.ListAllMyBucketsResult result = await _minioClient.ListBucketsAsync();

            return result.Buckets.AsReadOnly().Select(selector: item => item.Name).ToList();
        }

        public async Task<(Guid ObjectId, string? ObjectUrl)?> GetObjectAsync(AvailableBucket bucket, (Guid objectId, string fileExtension) fileName)
        {
            bool objectExisted = await IsObjectExistedAsync(bucket, fileName.objectId, fileName.fileExtension);

            if (!objectExisted) { return null; }

            try
            {
                (Guid objectId, string fileExtension) = fileName;

                DateTime currentTime = DateTime.Now;

                PresignedGetObjectArgs pgoArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucket: GetBucketName(bucket))
                    .WithObject(obj: $"{objectId}.{fileExtension}")
                    .WithExpiry(expiry: (60 - currentTime.Second) * (60 - currentTime.Minute) * (24 - currentTime.Hour));

                return (ObjectId: objectId, ObjectUrl: await _minioClient.PresignedGetObjectAsync(args: pgoArgs));
            }
            catch { return null; }
        }

        public async Task<ICollection<(Guid ObjectId, string ObjectUrl)>?> GetObjectCollectionAsync(AvailableBucket bucket, ICollection<(Guid objectId, string fileExtension)> objectCollection)
        {
            try
            {
                List<(Guid ObjectId, string ObjectUrl)> result = new();

                foreach ((Guid objectId, string fileExtension) fileName in objectCollection)
                {
                    (Guid ObjectId, string? ObjectUrl)? resultItem = await GetObjectAsync(bucket, fileName);

                    if (resultItem != null) { result.Add(resultItem.Value!); }
                }

                return result;
            }
            catch { return null; }
        }

        public Task<bool> RemoveObjectAsync(AvailableBucket bucket, Guid objectId)
        {
            throw new NotImplementedException();
        }

        private async Task<string> EnsureBucketExistedAsync(AvailableBucket bucket)
        {
            string bucketName = await IsBucketExistedAsync(bucket);

            if (!string.IsNullOrEmpty(bucketName))
            {
                return bucketName;
            }

            try
            {
                bucketName = GetBucketName(bucket);

                MakeBucketArgs mbArgs = new MakeBucketArgs().WithBucket(bucket: bucketName);

                await _minioClient.MakeBucketAsync(args: mbArgs);
            }
            catch { return string.Empty; }

            return bucketName;
        }

        private async Task<bool> IsObjectExistedAsync(AvailableBucket bucket, Guid objectId, string fileExtension)
        {
            string bucketName = await EnsureBucketExistedAsync(bucket);

            try
            {
                StatObjectArgs soArgs = new StatObjectArgs()
                    .WithBucket(bucket: bucketName)
                    .WithObject(obj: $"{objectId}.{fileExtension}");

                Minio.DataModel.ObjectStat existingObject = await _minioClient.StatObjectAsync(args: soArgs);

                return existingObject != null;
            }
            catch { return false; }
        }

        private async Task<string> IsBucketExistedAsync(AvailableBucket bucket)
        {
            string bucketName = GetBucketName(bucket);
            try
            {
                BucketExistsArgs beArgs = new BucketExistsArgs().WithBucket(bucket: bucketName);

                bool bucketExisted = await _minioClient.BucketExistsAsync(args: beArgs);

                return bucketExisted ? bucketName : string.Empty;
            }
            catch { return string.Empty; }
        }

        private static string GetBucketName(AvailableBucket bucket)
        {
            return bucket.GetDisplayName().ToLower();
        }
    }
}