using System.Threading.Tasks;

namespace AwsDotnetCsharp
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(string finalKey, params string[] toAddresses);
        Task<bool> SendErrorEmail(params string[] toAddresses);
    }
}