﻿using System;
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

            var standardsImportTask =  _standardsImportService.ImportDataIntoStaging();
            var larsImportTask =  _larsImportService.ImportDataIntoStaging();
            var frameworksImportTask = _frameworksImportService.ImportDataIntoStaging();

            await Task.WhenAll(larsImportTask, standardsImportTask, frameworksImportTask);

            var loadTasks = new List<Task>();

            var frameworkImportResponse = frameworksImportTask.Result;
            if (frameworkImportResponse.Success) loadTasks.Add(_frameworksImportService.LoadDataFromStaging(importStartTime, frameworkImportResponse.LatestFile));

            var standardsImportResponse = standardsImportTask.Result;
            if (standardsImportResponse) loadTasks.Add(_standardsImportService.LoadDataFromStaging(importStartTime));

            if (loadTasks.Count > 0)
            {
                await Task.WhenAll(loadTasks);
            }
            
            var larsImportResponse = larsImportTask.Result;
            if (larsImportResponse.Success)  await _larsImportService.LoadDataFromStaging(importStartTime, larsImportResponse.FileName);

            _indexBuilder.Build();

            return Unit.Value;
        }
    }
}
