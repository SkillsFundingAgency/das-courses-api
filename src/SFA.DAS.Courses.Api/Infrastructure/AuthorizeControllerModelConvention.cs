using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using SFA.DAS.Courses.Api.Controllers;

namespace SFA.DAS.Courses.Api.Infrastructure
{
    public class AuthorizeControllerModelConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Filters.Add(controller.ControllerType.Name.Equals(nameof(ImportController))
                ? new AuthorizeFilter(PolicyNames.HasDataLoadPolicy)
                : new AuthorizeFilter(PolicyNames.Default));
        }
    }
}