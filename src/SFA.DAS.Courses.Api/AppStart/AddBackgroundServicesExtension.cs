using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Courses.Api.TaskQueue;
using SFA.DAS.Courses.Domain.Configuration;

namespace SFA.DAS.Courses.Api.AppStart
{
    public static class AddBackgroundServicesExtension
    {
        public static void AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<TaskQueueHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        }
    }
}
