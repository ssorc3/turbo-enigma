using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using static Amazon.Lambda.SNSEvents.SNSEvent;

namespace AwsDotnetCsharp
{
    public class RequestHandler
    {
        private readonly SNSPublisher _publisher;
        private readonly IFileRepository _fileRepo;

        public RequestHandler()
        {
            _publisher = new SNSPublisher();
            _fileRepo = new S3FileRepository();
        }

        public async Task<bool> HandleRequest(IList<SNSRecord> records)
        {
            var success = false;
            foreach(var record in records)
            {
                //edit image and save result in memory as a PNG
                MemoryStream outputStream;
                using(var stream = await _fileRepo.GetObject(record.Sns.Message))
                {
                    var image = Image.Load(stream);
                    image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2).DetectEdges());
                    outputStream = new MemoryStream();
                    image.Save(outputStream, new PngEncoder());
                }

                var finalKey = GenerateFinalKey(record.Sns.Message);

                //upload the result to S3
                var putObjectSuccess = await _fileRepo.PutObject(outputStream, finalKey);
                outputStream.Dispose();

                //get email from the tag of the original file
                var tags = await _fileRepo.GetTags(record.Sns.Message);
                var email = tags["email"];
                var message = JsonConvert.SerializeObject(new {key=finalKey, email=email});
                LambdaLogger.Log(message);
                await _publisher.Publish(message);
                success = true;
            }
            return success;
        }

        private string GenerateFinalKey(string originalKey)
        {
            return "images/after/" + originalKey.Split('/').Last();
        }
    }
}