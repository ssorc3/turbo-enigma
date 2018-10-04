using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Threading.Tasks;
using System.Linq;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        private static readonly string SNS_ARN = (string)Environment.GetEnvironmentVariable("SNS_ARN");
        private readonly IAmazonSimpleNotificationService _snsClient;
        
        public Handler()
        {
            _snsClient = new AmazonSimpleNotificationServiceClient();
        }

        public async Task Handle(S3Event s3Event, ILambdaContext context)
        {
            if(!s3Event.Records.Any()) context.Logger.LogLine("No records");
            foreach(var record in s3Event.Records)
            {
                try
                {
                    context.Logger.LogLine($"Started publish request for {record.S3.Object.Key} to {SNS_ARN}");
                    var publishRequest = new PublishRequest(SNS_ARN, record.S3.Object.Key);
                    var publishResponse = await _snsClient.PublishAsync(publishRequest).ConfigureAwait(false);
                    context.Logger.LogLine($"Published message for {record.S3.Object.Key}");
                }
                catch(Exception e)
                {
                    context.Logger.LogLine($"Failed to publish SNS notification for {record.S3.Object.Key} with error: {e.Message}");
                }
            }
        }
    }
}
