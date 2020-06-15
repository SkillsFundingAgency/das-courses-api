using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Api.Infrastructure
{
    public static class PolicyNames
    {
        public static string DataLoad => nameof(DataLoad);
        public static string Default => nameof(Default);
        
        public static readonly List<string> PolicyNameList = new List<string>
        {
            DataLoad
        };
    }
}