using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;

namespace AwsDotnetCsharp
{
    public class SESEmailSender : IEmailSender
    {
        private readonly IAmazonSimpleEmailService _sesClient;

        public SESEmailSender()
        {
            _sesClient = new AmazonSimpleEmailServiceClient();
        }

        public async Task<bool> SendEmail(string finalKey, params string[] toAddresses)
        {
            var emailRequest = EmailRequestBuilder.GenerateSuccessEmail(finalKey, toAddresses);
            var response = await _sesClient.SendEmailAsync(emailRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> SendErrorEmail(params string[] toAddresses)
        {
            var emailRequest = EmailRequestBuilder.GenerateErrorEmail(toAddresses);
            var response = await _sesClient.SendEmailAsync(emailRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}