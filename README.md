# Heroes - Dotnet

It is backend for tour of heroes angular app. Angular part of application can be found in [this repository](https://github.com/Bielik20/heroes-angular).

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

In root directory
```
dotnet run -p Server/Server.csproj
```
For some reason there are still some bugs in CLI and this command doesn't work. The way to work it around is to use either Visual Studio or Visual Studio Code with C# Extention and run it in debug mode.

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

More information can be found in [documentation](https://docs.microsoft.com/en-us/aspnet/core/security/cors).