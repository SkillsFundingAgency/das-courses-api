using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Courses.Api.TaskQueue
{
    public interface IBackgroundRequest
    {
        Guid RequestId { get; }

        string RequestName { get; }

        Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken);
    }
}
