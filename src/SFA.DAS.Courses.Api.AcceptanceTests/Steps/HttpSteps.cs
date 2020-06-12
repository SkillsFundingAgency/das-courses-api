using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Steps
{
    [Binding]
    public class HttpSteps
    {
        private readonly ScenarioContext _context;

        public HttpSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Given("I have an http client")]
        public void GivenIHaveAnHttpClient()
        {
            var client = new AcceptanceTestingWebApplicationFactory<Startup>().CreateClient();
            _context.Set(client,ContextKeys.HttpClient);
        }

        [When("I (GET|POST|PUT|DELETE) the following url: (.*)")]
        public async Task WhenIMethodTheFollowingUrl(HttpMethod method, string url)
        {
            var client = _context.Get<HttpClient>(ContextKeys.HttpClient);

            HttpResponseMessage response = null;
            switch (method)
            {
                case HttpMethod.Get:
                    response = await client.GetAsync(url);
                    break;
                case HttpMethod.Post:
                    if (!_context.TryGetValue<HttpContent>(ContextKeys.HttpResponse, out var postContent))
                    {
                        Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpContent}]");
                    }
                    response = await client.PostAsync(url, postContent);
                    break;
                case HttpMethod.Put:
                    if (!_context.TryGetValue<HttpContent>(ContextKeys.HttpResponse, out var putContent))
                    {
                        Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpContent}]");
                    }
                    response = await client.PutAsync(url, putContent);
                    break;
                case HttpMethod.Delete:
                    response = await client.DeleteAsync(url);
                    break;
                default:
                    Assert.Fail($"case for http method [{method}] has not been implemented");
                    break;
            }

            _context.Set(response, ContextKeys.HttpResponse);
        }

        [Then("an http status code of (.*) is returned")]
        public void ThenAnHttpStatusCodeIsReturned(int httpStatusCode)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            result.StatusCode.Should().Be(httpStatusCode);
        }
    }
}
