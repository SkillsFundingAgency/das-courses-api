using System;
using AutoFixture;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    public class StandardCustomization : ICustomization
    {
        private readonly string _larsCode;
        private readonly string _status;
        private readonly ApprenticeshipType? _apprenticeshipType;
        private readonly CourseType? _courseType;
        private readonly string _version;
        private readonly DateTime? _approvedForDelivery;
        private readonly DateTime? _effectiveFrom;
        private readonly DateTime? _effectiveTo;
        private readonly DateTime? _lastDateStarts;

        public StandardCustomization(
            string larsCode = null,
            string status = null,
            ApprenticeshipType? apprenticeshipType = null,
            CourseType? courseType = null,
            string version = null,
            DateTime? approvedForDelivery = null,
            DateTime? effectiveFrom = null,
            DateTime? effectiveTo = null,
            DateTime? lastDateStarts = null)
        {
            _larsCode = larsCode;
            _status = status;
            _apprenticeshipType = apprenticeshipType;
            _courseType = courseType;
            _version = version;
            _approvedForDelivery = approvedForDelivery;
            _effectiveFrom = effectiveFrom;
            _effectiveTo = effectiveTo;
            _lastDateStarts = lastDateStarts;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customize<SectorSubjectAreaTier1>(composer =>
            {
                return composer
                    .Without(x => x.LarsStandards);
            });

            fixture.Customize<SectorSubjectAreaTier2>(composer =>
            {
                return composer
                    .Without(x => x.LarsStandard);
            });

            fixture.Customize<LarsStandard>(composer =>
            {
                var sectorSubjectArea1 = fixture.Create<SectorSubjectAreaTier1>();
                var sectorSubjectArea2 = fixture.Create<SectorSubjectAreaTier2>();

                var result = composer
                    .With(x => x.SectorSubjectArea1, sectorSubjectArea1)
                    .With(x => x.SectorSubjectAreaTier1, sectorSubjectArea1.SectorSubjectAreaTier1)
                    .With(x => x.SectorSubjectArea2, sectorSubjectArea2)
                    .With(x => x.SectorSubjectAreaTier2, sectorSubjectArea2.SectorSubjectAreaTier2);

                if (_effectiveFrom.HasValue)
                {
                    result = result.With(x => x.EffectiveFrom, _effectiveFrom.Value);
                }

                result = _effectiveTo.HasValue
                    ? result.With(x => x.EffectiveTo, _effectiveTo.Value)
                    : result.Without(x => x.EffectiveTo);

                result = _lastDateStarts.HasValue
                    ? result.With(x => x.LastDateStarts, _lastDateStarts.Value)
                    : result.Without(x => x.LastDateStarts);

                return result;
            });

            fixture.Customize<ShortCourseDates>(composer =>
            {
                var result = composer
                    .With(x => x.LarsCode);

                if (_effectiveFrom.HasValue)
                {
                    result = result.With(x => x.EffectiveFrom, _effectiveFrom.Value);
                }

                result = _effectiveTo.HasValue
                    ? result.With(x => x.EffectiveTo, _effectiveTo.Value)
                    : result.Without(x => x.EffectiveTo);

                result = _lastDateStarts.HasValue
                    ? result.With(x => x.LastDateStarts, _lastDateStarts.Value)
                    : result.Without(x => x.LastDateStarts);

                return result;
            });

            fixture.Customize<Standard>(composer =>
            {
                var result = composer
                    .With(standard => standard.Status, _status)
                    .With(standard => standard.ApprenticeshipType, _apprenticeshipType)
                    .With(standard => standard.CourseType, _courseType)
                    .With(standard => standard.Version, _version)
                    .With(standard => standard.ApprovedForDelivery, _approvedForDelivery);
                
                if(_courseType == CourseType.Apprenticeship)
                {
                    if (_larsCode != "0")
                    {
                        var larsStandard = fixture.Create<LarsStandard>();

                        result = result
                            .With(standard => standard.LarsCode, larsStandard.LarsCode)
                            .With(standard => standard.LarsStandard, larsStandard);
                    }
                    else
                    {
                        result = result
                            .With(standard => standard.LarsCode, _larsCode);
                    }

                    result = result
                        .Without(standard => standard.ShortCourseDates);
                }
                else if(_courseType == CourseType.ShortCourse)
                {
                    if (_larsCode != string.Empty)
                    {
                        var shortCourseDates = fixture.Create<ShortCourseDates>();

                        result = result
                            .With(standard => standard.LarsCode, shortCourseDates.LarsCode)
                            .With(standard => standard.ShortCourseDates, shortCourseDates);
                    }
                    else
                    {
                        result = result
                            .With(standard => standard.LarsCode, _larsCode);
                    }

                    result = result
                        .Without(standard => standard.LarsStandard);
                }

                return result;
            });
        }
    }
}
