# das-courses-api

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-courses-api?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2187&branchName=master)

## Requirements

- DotNet Core 2.2 and any supported IDE for DEV running.
- *If you are not wishing to run the in memory database then*
- SQL Server database.
- Azure Storage Account

## About

das-courses-api represents the inner api definition for apprenticeship standards, with data taken from IFATE and LARS. The API creates a local copy of the data for querying over.

## Local running

### In memory database
It is possible to run the whole of the API using the InMemory database. To do this the environment variable in appsettings.json should be set to **DEV**. 
Once done, start the application as normal, then run the ```ops/dataload``` operation in swagger. You will then be able to query the API
as per the operations listed in swagger.

### SQL Server database
You are able to run the API by doing the following:

* Run the database deployment publish command to create the database ```SFA.DAS.Courses``` or create the database manually and run in the table creation scripts
* In your Azure Storage Account, create a table called Configuration and Add the following
```
ParitionKey: LOCAL
RowKey: SFA.DAS.Courses_1.0
data: {"Courses":{"ConnectionString":"DBCONNECTIONSTRING"}}
```
* Start the api project ```SFA.DAS.Courses.Api```

Sending the following to the API

```POST /ops/dataload```

will load data from IFATE into the local database.

Starting the API will then show the swagger definition with the available operations.
