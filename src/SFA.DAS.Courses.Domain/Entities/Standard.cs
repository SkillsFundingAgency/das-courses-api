﻿namespace SFA.DAS.Courses.Domain.Entities
{
    public class Standard : StandardBase
    {
        public float? SearchScore { get; set; }

        public static implicit operator Standard(StandardImport standard)
        {
            return new Standard
            {
                Id = standard.Id,
                LarsCode = standard.LarsCode,
                IfateReferenceNumber = standard.IfateReferenceNumber,
                Status = standard.Status,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl,
                RouteId = standard.RouteId,
                Title = standard.Title,
                Sector = standard.Sector,
                TypicalJobTitles = standard.TypicalJobTitles,
                Version = standard.Version,
                Keywords = standard.Keywords,
                RegulatedBody = standard.RegulatedBody,
                Skills = standard.Skills,
                Knowledge = standard.Knowledge,
                Behaviours = standard.Behaviours,
                Duties = standard.Duties,
                CoreAndOptions = standard.CoreAndOptions,
                CoreDuties = standard.CoreDuties,
                IntegratedApprenticeship = standard.IntegratedApprenticeship
            };
        }
    }
}
