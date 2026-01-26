using System;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure
{

    [Binding]
    public sealed class Hooks
    {
        private readonly ScenarioContext _context;

        public Hooks(ScenarioContext context)
        {
            _context = context;
        }

        [AfterScenario(Order = 1000)]
        public void TearDown()
        {
            if (_context.TryGetValue<HttpClient>(ContextKeys.HttpClient, out var client))
            {
                client.Dispose();
            }

            if (_context.TryGetValue<IDisposable>(ContextKeys.WebAppFactory, out var factory))
            {
                factory.Dispose();
            }
        }
    }
}
