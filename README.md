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

## Enable Cors

For local hosts to work together there is need to enable Cors. First install [Chrome Extension](https://chrome.google.com/webstore/detail/allow-control-allow-origi/nlfbmbojpeacfghkpbjhddihlkkiljbi)

In the Startup class:
- ConfigureServices:
```cs
services.AddCors();
```

- Configure, before `app.UseMvc()`
```cs
app.UseCors(builder => builder.WithOrigins("http://localhost:4200"));
```