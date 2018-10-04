using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using HttpMultipartParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        private readonly RequestService _requestService;
        public Handler()
        {
            _requestService = new RequestService();
        }

        public async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var email = request.QueryStringParameters["email"];
            var bytes = Convert.FromBase64String(request.Body.Split(',')[1]);
            bool success;
            using (var stream = new MemoryStream(bytes))
            {
                success = await _requestService.PutObject(email, stream);
            }

            var response = new APIGatewayProxyResponse()
            {
                Body = JsonConvert.SerializeObject(success ? new {message="Upload Successful"} : new {message="Error uploading image"}),
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string>()
                {
                    {"Content-Type", "application/json"},
                    {"Access-Control-Allow-Origin", "*"},
                    {"Access-Control-Allow-Credentials", "true"}
                }
            };
            return response;
        }
    }
}
