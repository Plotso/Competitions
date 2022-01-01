# Competitions
Web platform focused on providing fast and easy way for people to participate in or organize new competitions. 

Small university project aiming towards adding SEO optimisation to an ASP.NET Core website. It's a primitive competitions website with no commercial use, it's more like social platform where people can create competitions (leagues/cup tournaments)  or participate in such. Each competition can have fee, prize and rules, all set by the organiser. Platform acts like a listing place where people can easily find past, ongoing and future competiitions.

## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

In order to be sure you can run the project, make sure you have the following frameworks installed on your PC:
* **NET Standard 2.1 compatible frameworks** - **NET Core 3.1** OR **NET 5** (should be installed with VisualStudio/Rider, double check it just in case)
* **SQL Server 2019** - the project is built and tested on EF 5.0.13 which, is using latest SQL Server version

It's good to have the following software products installed in order to be sure the project is running as expected:
* **VisualStudio 2019 or later / Rider 2020 or later** - built and tested on both of those IDEs, the project should also be running on any newer version as long as it supports the above mentioned frameworks
* **SQL Server Management Studio (SSMS) / Azure Data Studio** - This one is not required at all, but if you want to check what happens in the database, it's good to have it. It's recommended to use latest version, the project has been tested on SSMS 2018 & Azure Data Studio 2020 and it's working fine.

### Installation

If you want to have **custom database name**, go to [appsettings.json](src/Web/Competitions.Web/appsettings.json) file and change **_THAT__** to whatever name you would like:
```
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=THAT_;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
```

Before initially running the project, go to [Competitions.Data](src/Data/Competitions.Data) folder (the one containing Competitions.Data.csproj file) and execute the following commands:

```
dotnet ef migrations add initialCreate
```
```
dotnet ef database update 
```
Those commands are required in order for the project to correctly build the database before running it. 
Once above installation is done, run the project either from the IDE or from console.
