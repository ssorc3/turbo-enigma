using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using SixLabors.ImageSharp.Formats.Png;
using Amazon.SimpleEmail;
using System.Net;
using Amazon.SimpleEmail.Model;
using System.Collections.Generic;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        private readonly RequestHandler _requestHandler;
        public Handler()
        {
            _requestHandler = new RequestHandler();
        }

        public async Task Handle(SNSEvent snsEvent, ILambdaContext context)
        {
            if (!snsEvent.Records.Any()) context.Logger.LogLine($"No records on event");
            if(!await _requestHandler.HandleRequest(snsEvent.Records))
            {
                LambdaLogger.Log("Error with one or more of the events.");
            }
        }
    }
}
