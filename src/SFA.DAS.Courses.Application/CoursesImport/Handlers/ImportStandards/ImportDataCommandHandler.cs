using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards
{
    public class ImportDataCommandHandler : IRequestHandler<ImportDataCommand, Unit>
    {
        private readonly IStandardsImportService _standardsImportService;
        private readonly ILarsImportService _larsImportService;
        private readonly IFrameworksImportService _frameworksImportService;
        private readonly IIndexBuilder _indexBuilder;

        public ImportDataCommandHandler (IStandardsImportService standardsImportService, ILarsImportService larsImportService, IFrameworksImportService frameworksImportService, IIndexBuilder indexBuilder)
        {
            _standardsImportService = standardsImportService;
            _larsImportService = larsImportService;
            _frameworksImportService = frameworksImportService;
            _indexBuilder = indexBuilder;
        }
        public async Task<Unit> Handle(ImportDataCommand request, CancellationToken cancellationToken)
        {
            var importStartTime = DateTime.Now;

            var standardsImport =  _standardsImportService.ImportDataIntoStaging();
            var larsImportResult =  _larsImportService.ImportDataIntoStaging();
            var frameworksResult = _frameworksImportService.ImportDataIntoStaging();

            await Task.WhenAll(larsImportResult, standardsImport, frameworksResult);

            var frameworkResponse = frameworksResult.Result;

            var tasks = new List<Task>();

            if (frameworkResponse.Success) tasks.Add(_frameworksImportService.LoadDataFromStaging(importStartTime, frameworkResponse.LatestFile));

            await Task.WhenAll(tasks);
            _indexBuilder.Build();
            
            return Unit.Value;
        }
    }
}
