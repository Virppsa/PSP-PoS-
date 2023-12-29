# PSP-PoS-
PoS system for software engineering course.

## Added - 2023-12-11
Sample project with crud. 

## To run locally
prerequisites:
- dotnet sdk 8.0
- dotnet ef cli

Edit [appsettings.json](./PspPos/appsettings.json) file: put your local mssql db connection string (AND DO NOT COMMIT IT!)
```
git pull
dotnet ef database update
dotnet run
```