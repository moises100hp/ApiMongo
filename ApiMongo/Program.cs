using ApiMongo.Infra;
using ApiMongo.Mappers;
using ApiMongo.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region [Cors]

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Origem do seu Angular
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Curso .NET Udemy", Version = "v1" });
    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
                    },
        Array.Empty<string>()
        }
    });
});

#region [Database]
builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetSection(nameof(DatabaseSetting)));
builder.Services.AddSingleton<IDatabaseSetting>(sp => sp.GetRequiredService<IOptions<DatabaseSetting>>().Value);
#endregion

#region [Cache]

builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetSection("Redis:ConnectionString").Value;
});

#endregion

#region [HealthCheck]

var baseUri = builder.Configuration.GetSection("DatabaseSetting:ConnectionString").Value;
var dbName = builder.Configuration.GetSection("DatabaseSetting:db_portal").Value;

var urlBuilder = new MongoUrlBuilder(baseUri)
{
    DatabaseName = dbName
};

var mongoUrl = urlBuilder.ToMongoUrl();

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetSection("Redis:ConnectionString").Value, tags: new string[] { "db", "data" })
    .AddMongoDb(
            sp => sp.GetRequiredService<IMongoClient>(),
            name: "mongodb", tags: ["db", "data"]);

builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(15); //time in seconds between check
    options.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
    options.SetApiMaxActiveRequests(1);//api request concurrency

    options.AddHealthCheckEndpoint("default api", "/health"); //map health check api
}).AddInMemoryStorage();


#endregion

#region [DI]

builder.Services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
builder.Services.AddSingleton(typeof(INewsService), typeof(NewsService));
builder.Services.AddSingleton(typeof(IVideoService), typeof(VideoService));
builder.Services.AddTransient(typeof(IUploadService), typeof(UploadService));
builder.Services.AddTransient(typeof(GalleryService));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    return new MongoClient(mongoUrl);
});

builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration.GetSection("DatabaseSetting:db_portal").Value;
    return client.GetDatabase(databaseName);
});

builder.Services.AddSingleton(typeof(IMemoryCache), typeof(MemoryCache));
//builder.Services.AddSingleton(typeof(ICacheService), typeof(CacheMemoryService));
builder.Services.AddSingleton(typeof(ICacheService), typeof(CacheRedisService));

#endregion

#region [AutoMapper]
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<EntityToViewModelMapping>();
    cfg.AddProfile<ViewModelToEntityMapping>();
}, AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region [JWT]

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(builder.Configuration.GetSection("tokenManagement:secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

#region [HealthCheck]

app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).UseHealthChecksUI(health => health.UIPath = "/healthui");

#endregion

app.UseHttpsRedirection();

#region [StatisFiles]

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Medias")),
    RequestPath = "/medias"
});

#endregion

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("MyCorsPolicy");

app.MapControllers();

app.Run();
