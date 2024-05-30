using Amazon.S3;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using CozyGames.Data;
using CozyGames.Helpers;
using CozyGames.Models;
using CozyGames.Repositories;
using CozyGames.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false);
builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<ServiceStorageAWS>();
//SEGURIDAD CON CLAIMS
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();
builder.Services.AddHttpContextAccessor();

//KEYVAULT
//builder.Services.AddAzureClients(factory =>
//{
//    factory.AddSecretClient
//        (builder.Configuration.GetSection("KeyVault"));
//});

// DEBEMOS PODER RECUPERAR UN OBJETO INYECTADO EN CLASES 
// QUE NO TIENEN CONSTRUCTOR 

//SecretClient secretClient = builder.Services.BuildServiceProvider()
//    .GetService<SecretClient>();
//KeyVaultSecret secretStorageAccount = await secretClient.GetSecretAsync("storagekey");
//string storageAccountKey = secretStorageAccount.Value;
//BlobServiceClient blobServiceClient = new BlobServiceClient(storageAccountKey);
//builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);
string jsonSecrets = await
    HelperSecretManager.GetSecretAsync();
KeysModel keysModel =
    JsonConvert.DeserializeObject<KeysModel>(jsonSecrets);
builder.Services.AddSingleton<KeysModel>(x => keysModel);

//string connectionString = builder.Configuration.GetConnectionString("SqlHospital");

//builder.Services.AddTransient<RepositoryGamesSqlServer>();
builder.Services.AddTransient<ServiceGames>();
//builder.Services.AddTransient<ServiceStorageBlobs>();
//builder.Services.AddTransient<RepositoryUsers>();

builder.Services.AddDistributedMemoryCache();
//builder.Services.AddDbContext<GamesContext>
//(options => options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Juegos}/{action=Index}/{id?}");
});


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Juegos}/{action=Index}");

app.Run();
