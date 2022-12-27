# MannsBlog

## Project Status

|What|Where|
|-----|-------------------------------------------------------------------------------------|
|Code  | [https://github.com/saigkill/saschamannsde] |
|Bugs & feature requests  | [https://github.com/saigkill/saschamannsde/issues] |
|The author's blog | [http://saschamanns.de] |

| What | Status |
|-------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|Code quality Codeclimate | [![Maintainability](https://api.codeclimate.com/v1/badges/b6604d7e1a7aad06183a/maintainability)](https://codeclimate.com/github/saigkill/saschamannsde/maintainability) |
|Code quality Sonarcloud | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=saigkill_saschamannsde&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=saigkill_saschamannsde)
|Continuous integration | ![.NET Core](https://github.com/saigkill/latex_curriculum_vitae-dotnet/workflows/.NET%20Core/badge.svg) |
|Downloads|![GitHub All Releases](https://img.shields.io/github/downloads/saigkill/saschamannsde/total)|
|Security check | ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/saigkill/saschamannsde/codeql.yml) |
|Open Issues | ![GitHub issues](https://img.shields.io/github/issues/saigkill/saschamannsde) |
|Open Pull Request | ![Pull Requests](https://img.shields.io/github/issues-pr/saigkill/saschamannsde) |
|Conventional commits | [![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-yellow.svg)](https://conventionalcommits.org) |
|License | ![GitHub](https://img.shields.io/github/license/saigkill/saschamannsde) |

I re-wrote my blog using a new stack of web technologies including:

 - .NET 7
 - ASP.NET Core 7
 - Entity Framework Core 7
 - Bootstrap 5
 - Azure Websites (Docker)
 - Azure Blob Storage
 - Syncfusion controls
 
## How to use

First of all, you need a valid Syncfusion License for the matching version number.

Then clone the repository.

I think i have abstracted the settings so far, that you can start to build your own blog.

Fill the shipped appsettings.json with your own credentials.

In the class "MannsInitializer" you can place some stuff, what will be created by seeding.

Create a DB and place the ConnectionString in appsettings.json.

Use the following steps:

dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef database update

So should the database exists with the given model.

Then run the app from commandline with command "/seed".

Inside the "Views" directory, you find my current Websites. You can modify it ayou like.