using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using AwsDotnetCsharp.Model;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
      private readonly IEmailSender _emailSender;
      public Handler()
      {
        _emailSender = new SESEmailSender();
      }

      public async Task SendEmail(SNSEvent input)
      {
        foreach(var notification in input.Records)
        {
          var emailNotification = JsonConvert.DeserializeObject<EmailNotification>(notification.Sns.Message);
          await _emailSender.SendEmail(emailNotification.Key, emailNotification.Email);
        }
      }
    }
}
