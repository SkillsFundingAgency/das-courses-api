﻿using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class GetStandardsListResult
    {
        public int Total { get; set; }
        public int TotalFiltered { get; set; }
        public IEnumerable<Standard> Standards { get; set; }
    }
}
