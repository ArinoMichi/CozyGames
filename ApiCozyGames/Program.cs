using ApiCozyGames.Data;
using ApiCozyGames.Helpers;
using ApiCozyGames.Repositories;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAzureClients(factory => 
{ 
    factory.AddSecretClient 
        (builder.Configuration.GetSection("KeyVault")); 
});

// DEBEMOS PODER RECUPERAR UN OBJETO INYECTADO EN CLASES 
// QUE NO TIENEN CONSTRUCTOR 

SecretClient secretClient = builder.Services.BuildServiceProvider()
    .GetService<SecretClient>();

KeyVaultSecret secret = await secretClient.GetSecretAsync("SqlAzure");
//AUTH USUARIOS
HelperActionServicesOAuth helper =
    new HelperActionServicesOAuth(secretClient);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());

string connectionString = secret.Value;
builder.Services.AddTransient<RepositoryGamesSqlServer>();
builder.Services.AddTransient<RepositoryUsers>();

builder.Services.AddDbContext<GamesContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "API COZY GAMES";
    document.Description = "API para CozyGames";
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();
app.UseOpenApi();
app.UseSwaggerUI(options =>
{
    options.InjectStylesheet("/css/theme-material.css");
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "CozyGames API");
    options.RoutePrefix = "";
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
