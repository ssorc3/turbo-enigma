using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;

namespace AwsDotnetCsharp
{
    public class RequestService
    {
        private static readonly string BUCKET_NAME = Environment.GetEnvironmentVariable("bucketName");
        private readonly IAmazonS3 _s3Client;

        public RequestService()
        {
            _s3Client = new AmazonS3Client();
        }
        
        public async Task<bool> PutObject(string email, Stream file)
        {
            try
            {
                var putObjectRequest = new PutObjectRequest()
                {
                    BucketName = BUCKET_NAME,
                    Key = $"images/before/{(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds}-{Guid.NewGuid()}.png",
                    InputStream = file,
                    TagSet = new List<Tag>
                    {
                        new Tag { Key = "email", Value = email }
                    }
                };
                var result = await _s3Client.PutObjectAsync(putObjectRequest).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                LambdaLogger.Log($"There was an error with the put object request: {e.Message}");
                return false;
            }

            return true;
        }
    }
}