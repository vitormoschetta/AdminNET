# AdminNET

### Add Project
```
dotnet new webapp --auth Individual -o src/AdminNET
```


### Add Migration
```
dotnet ef migrations add initial --project src/AdminNET/AdminNET.csproj -o Data/Migrations
```


### Scaffold Identity Pages
```
dotnet aspnet-codegenerator identity -dc AdminNET.Data.ApplicationDbContext --files "Account.Register;Account.Login;Account.Logout;Account.Manage.Index;Account.Manage.ChangePassword" --project src/AdminNET/AdminNET.csproj
```
Ou, para gerar todas as páginas de identidade:
```
dotnet aspnet-codegenerator identity -dc AdminNET.Data.ApplicationDbContext --project src/AdminNET/AdminNET.csproj
```


### Personalizar classes de usuário e papel

- Criar as classes ApplicationUser em src/AdminNET/Areas/Identity/Models, estendendo IdentityUser;
- Alterar a classe ApplicationDbContext em src/AdminNET/Data/ApplicationDbContext, estendendo IdentityDbContext<ApplicationUser>;


```
dotnet aspnet-codegenerator identity -dc AdminNET.Data.ApplicationDbContext --files "Account.Register;Account.Login;Account.Logout;Account.Manage.Index;Account.Manage.ChangePassword" --project src/AdminNET/AdminNET.csproj --userclass ApplicationUser --usernamespace AdminNET.Models --contextnamespace AdminNET.Data --contextdir Data --layout _Layout
```



### Referencia 

https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=netcore-cli

