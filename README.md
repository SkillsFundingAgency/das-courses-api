# das-courses-api

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-courses-api?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2187&branchName=master)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-courses-api&metric=alert_status)](https://sonarcloud.io/dashboard?id=SkillsFundingAgency_das-courses-api)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## ?? Installation

### Pre-Requisites
* A clone of this repository(https://github.com/SkillsFundingAgency/das-courses-api)
* A code editor that supports .NetCore 8 and above
* A storage emulator like Azurite (https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* An Azure Active Directory account with the appropriate roles as per the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-courses-api/SFA.DAS.Courses.Api.json)
* SQL Server database.


## About

das-courses-api represents the inner api definition for apprenticeship standards, with data taken from IFATE and LARS. The API creates a local copy of the data for querying over. The API also runs a RAM version of Lucene, to allow keyword searching of courses by **Title**, **Keyword** and **Typical Job Titles**.

## Local running

### In memory database
It is possible to run the whole of the API using the InMemory database. To do this the environment variable in appsettings.json should be set to **DEV**. 
Once done, start the application as normal, then run the ```ops/dataload``` operation in swagger. You will then be able to query the API
as per the operations listed in swagger.

### SQL Server database
You are able to run the API by doing the following:

* Run the database deployment publish command to create the database ```SFA.DAS.Courses``` or create the database manually and run in the table creation scripts
* In your Azure Storage Account, create a table called Configuration and Add the following

ParitionKey: LOCAL
RowKey: SFA.DAS.Courses.Api_1.0
Data:
```
{
	"Courses":{
		"ConnectionString":"DBCONNECTIONSTRING"
	},
	"AzureAd": {
    "tenant": "********.onmicrosoft.com",
    "identifier": "https://********.onmicrosoft.com/******"
  }
}
```
* Start the api project ```SFA.DAS.Courses.Api```

## How It Works

### Running

* Open command prompt and change directory to _**/src/SFA.DAS.Courses.Api/**_
* Run the web project _**/src/SFA.DAS.Courses.Api/SFA.DAS.Courses.Api.csproj**_

MacOS
```
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

### Application logs
Application logs are logged to [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) and can be viewed using [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/overview) at https://portal.azure.com

## Useful URLs

### Courses Api

Once started, select the `Operations V1` defintion in swagger, and execute the following endpoint

```POST /ops/dataload```

This will load data from IFATE, LARS and a static Frameworks data file, into the local database.

Switching back to the CoursesApi defintion will then allow you to exectue the other endpoints. Note that if you are running using InMemory then you will need to load the data on each run.

Here are the definitions for the API endpoints in the StandardsController:

GetList
Endpoint: GET /api/courses/standards
Description: Retrieves a list of standards based on the provided query parameters.


Get
Endpoint: GET /api/courses/standards/{id}
Description: Retrieves the details of a specific standard by its ID.


GetOptionKsbs
Endpoint: GET /api/courses/standards/{id}/options/{option}/ksbs
Description: Retrieves the knowledge, skills, and behaviors (KSBs) for a specific option of a standard.

GetStandardsByIFateReferenceNumber
Endpoint: GET /api/courses/standards/versions/{iFateReferenceNumber}
Description: Retrieves a list of standards by their iFate reference number.


## License

Licensed under the [MIT license](LICENSE)
