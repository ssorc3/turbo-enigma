using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AwsDotnetCsharp
{
    public interface IFileRepository
    {
        Task<Stream> GetObject(string key);
        Task<Dictionary<string,string>> GetTags(string key);
        Task<bool> PutObject(Stream file, string fileName);
    }
}