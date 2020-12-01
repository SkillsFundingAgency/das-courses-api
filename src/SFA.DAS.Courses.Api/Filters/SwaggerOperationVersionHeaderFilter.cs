using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SFA.DAS.Courses.Api.Filters
{
    public class SwaggerOperationVersionHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            var version = context.ApiDescription.GroupName.Replace("v", string.Empty);

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Version",
                In = ParameterLocation.Header,
                AllowEmptyValue = false,
                Example = new OpenApiString(int.TryParse(version, out int ver) ? ver.ToString("#.0") : "1.0"),
                Required = true
            });
        }
    }
}
