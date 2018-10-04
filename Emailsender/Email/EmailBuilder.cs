using System;
using System.Collections.Generic;
using Amazon.SimpleEmail.Model;

namespace AwsDotnetCsharp
{
    public static class EmailRequestBuilder
    {
        private static readonly string FROM_EMAIL = Environment.GetEnvironmentVariable("fromEmail");
        private static readonly string REGION = Environment.GetEnvironmentVariable("region");
        private static readonly string BUCKET_NAME = Environment.GetEnvironmentVariable("bucketName");

        public static SendEmailRequest GenerateSuccessEmail(string finalKey, params string[] toAddresses)
        {
            return GenerateEmailWithMessage("Your new image is ready", $"Your new image is ready!<br/><br/><a href='https://s3-{REGION}.amazonaws.com/{BUCKET_NAME}/{finalKey}'>Click here!</a>", toAddresses);
        }

        public static SendEmailRequest GenerateErrorEmail(params string[] toAddresses)
        {
            return GenerateEmailWithMessage("Something went wrong...", "There was an error in editing your image. Please try again later.", toAddresses);
        }

        private static SendEmailRequest GenerateEmailWithMessage(string subject, string message, string[] toAddresses)
        {
            return new SendEmailRequest()
            {
                Source = FROM_EMAIL,
                Destination = new Destination
                {
                    ToAddresses = new List<string>(toAddresses)
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = message
                    }
                    }
                }
            };
        }
    }
}