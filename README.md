# Heroes - Dotnet

It is backend for tour of heroes angular app.

## Initialization

In root directory:
```
dotnet restore
```

In Entities directory:
```
dotnet ef --startup-project ../Server migraitons add InitialCreate
dotnet ef --startup-project ../Server database update
```

In Server directory
```
dotnet run
```