using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace AwsDotnetCsharp
{
    public class SNSPublisher
    {
        private readonly string SNS_ARN = Environment.GetEnvironmentVariable("SNS_ARN");
        private readonly IAmazonSimpleNotificationService _snsClient;

        public SNSPublisher()
        {
            _snsClient = new AmazonSimpleNotificationServiceClient();
        }
        public async Task Publish(string message)
        {
            try
                {
                    LambdaLogger.Log($"Started publish request: {message} to {SNS_ARN}");
                    var publishRequest = new PublishRequest(SNS_ARN, message);
                    var publishResponse = await _snsClient.PublishAsync(publishRequest).ConfigureAwait(false);
                    LambdaLogger.Log($"Published message: {message}");
                }
                catch(Exception e)
                {
                    LambdaLogger.Log($"Failed to publish SNS notification: {message} with error: {e.Message}");
                }
        }
    }
}