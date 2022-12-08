# AdminNET

### Add Project
```
dotnet new webapp --auth Individual -o src/AdminNET
```


### Personalizar classes de usuário e papel

- Criar as classes ApplicationUser em src/AdminNET/Areas/Identity/Models, estendendo IdentityUser;
- Alterar a classe ApplicationDbContext em src/AdminNET/Data/ApplicationDbContext, estendendo IdentityDbContext<ApplicationUser>;
- Alterar a classe Program em src/AdminNET/Program, Adicionando o Identity com a classe personalizada ApplicationUser:
``` 
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
```


### Add Migration
```
dotnet ef migrations add initial --project src/AdminNET/AdminNET.csproj -o Data/Migrations
```


### Scaffold Identity Pages

Páginas de identidade específicas:
```
cd src/AdminNET
dotnet aspnet-codegenerator identity -dc AdminNET.Data.ApplicationDbContext --files "Account.Register;Account.Login;Account.Logout;Account.Manage.Index;Account.Manage.ChangePassword"
```

Todas as páginas de identidade:
```
dotnet aspnet-codegenerator identity -dc AdminNET.Data.ApplicationDbContext
```

Obs: As vezes, durante o scaffolding, a classe Program é alterada e o Identity é adicionado novamente, então é necessário remover o Identity e adicionar novamente com a classe ApplicationUser personalizada.


### Add Identity Pages to CRUD


### Add Todo Model scaffold razor pages

```
cd src/AdminNET
dotnet aspnet-codegenerator razorpage -m Todo -dc AdminNET.Data.ApplicationDbContext -udl -outDir Pages/Todos --referenceScriptLibraries
```



### Referencia 

https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=netcore-cli

