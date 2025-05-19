using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.Exceptions;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class SlackNotificationService : ISlackNotificationService
    {
        private readonly HttpClient _client;
        private readonly string _channel;
        private readonly string _user;

        public SlackNotificationService(IOptions<SlackNotificationConfiguration> options, HttpClient httpClient)
        {
            _client = httpClient;
            
            var configuration = options?.Value;
            if (configuration != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.BotUserOAuthToken);
                _channel = configuration.Channel;
                _user = configuration.User;
            }
        }

        public async Task UploadFile(List<string> content, string fileName, string message)
        {
            using var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
            {
                foreach (var line in content)
                {
                    await writer.WriteLineAsync(line);
                }
                await writer.FlushAsync();
            }
            memoryStream.Position = 0;

            // prepare to upload the file to Slack
            var getUploadUrlPayload = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("filename", fileName),
                new KeyValuePair<string, string>("length", memoryStream.Length.ToString())
            });

            var uploadUrlResponse = await _client.PostAsync(Constants.SlackStartUploadUrl, getUploadUrlPayload);
            var uploadUrlResponseString = await uploadUrlResponse.Content.ReadAsStringAsync();
            var uploadUrlJson = JsonConvert.DeserializeObject<dynamic>(uploadUrlResponseString);

            if (uploadUrlJson == null || !(bool)uploadUrlJson.ok)
            {
                throw new SlackNotificationException($"Failed to get Slack upload URL: {uploadUrlJson?.error}");
            }

            string uploadUrl = uploadUrlJson.upload_url;
            string fileId = uploadUrlJson.file_id;

            // upload the file to Slack
            using var fileUploadContent = new StreamContent(memoryStream);
            var multipartFormContent = new MultipartFormDataContent
            {
                { 
                    new ByteArrayContent(memoryStream.ToArray()), 
                    "file", 
                    fileName
                }
            };

            var fileUploadResponse = await _client.PostAsync(uploadUrl, multipartFormContent);
            if (!fileUploadResponse.IsSuccessStatusCode)
            {
                throw new SlackNotificationException("File upload to Slack failed.");
            }

            // complete the upload by notifiying channel with message
            var completeUploadPayload = new
            {
                files = new[]
                {
                    new
                    {
                        id = fileId,
                        title = fileName
                    }
                },
                channel_id = _channel,
                initial_comment = message
            };

            var completeUploadContent = new StringContent(
                JsonConvert.SerializeObject(completeUploadPayload),
                Encoding.UTF8,
                "application/json"
            );

            var completeUploadResponse = await _client.PostAsync(Constants.SlackCompleteUploadUrl, completeUploadContent);
            var completeUploadResponseString = await completeUploadResponse.Content.ReadAsStringAsync();
            var completeUploadJson = JsonConvert.DeserializeObject<dynamic>(completeUploadResponseString);

            if (completeUploadJson == null || !(bool)completeUploadJson.ok)
            {
                throw new SlackNotificationException($"Failed to complete Slack file upload: {completeUploadJson?.error}");
            }
        }

        public string FormattedTag()
        {
            return $"<{_user}>";
        }
    }
}
