## Clean Architecture Blazor WebAssembly Application Solution Template

This is a solution template for creating a Blazor WebAssembly application hosted on ASP.NET 6.0 and following the principles of Clean Architecture.

#### Feature includes

1. KeyVault for Secrets
2. B2C integration
3. Clean Architecture
4. Backend For Fronted (_Cookie based authentication and CSP (Content Security Policy)_)
5. CQRS (_Command Query Request Segregation_)
6. Retry Policy with Key rotation
7. Decoupling using Mediator
8. NSwag

## Getting started
You will need to install the .NET 6.0 SDK and the new template before you can create a project. Once .NET 6 is installed, open a new console window and run the following command to install the template:

```
dotnet new install Blazor.CleanArchitechture.B2C::2.0.1
```

After the template has been installed successfully, you can create a new project using the following command

```
dotnet new blazor-ca-sln -n myApp
```
