using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace AwsDotnetCsharp
{
    public class S3FileRepository : IFileRepository
    {
        private readonly string BUCKET_NAME = Environment.GetEnvironmentVariable("bucketName");
        private readonly IAmazonS3 _s3Client;

        public S3FileRepository()
        {
            _s3Client = new AmazonS3Client();
        }

        public async Task<Stream> GetObject(string key)
        {
            var getObjectRequest = new GetObjectRequest()
            {
                BucketName = BUCKET_NAME,
                Key = key
            };
            var response = await _s3Client.GetObjectAsync(getObjectRequest);
            return response.ResponseStream;
        }

        public async Task<Dictionary<string, string>> GetTags(string key)
        {
            var getTagsRequest = new GetObjectTaggingRequest()
            {
                BucketName = BUCKET_NAME,
                Key = key
            };
            var response = await _s3Client.GetObjectTaggingAsync(getTagsRequest);

            var result = new Dictionary<string, string>();
            foreach(var tag in response.Tagging)
            {
                result.Add(tag.Key, tag.Value);
            }
            return result;
        }

        public async Task<bool> PutObject(Stream file, string fileName)
        {
            var putObjectRequest = new PutObjectRequest()
            {
                BucketName = BUCKET_NAME,
                Key = fileName,
                InputStream = file,
                CannedACL = S3CannedACL.PublicRead
            };
            var response = await _s3Client.PutObjectAsync(putObjectRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}