using ApiMongo.Infra;
using ApiMongo.Mappers;
using ApiMongo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {  Title = "Curso .NET Udemy", Version = "v1" });
});

#region [Database]
builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetSection(nameof(DatabaseSetting)));
builder.Services.AddSingleton<IDatabaseSetting>(sp => sp.GetRequiredService<IOptions<DatabaseSetting>>().Value);
#endregion

#region [DI]
builder.Services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
builder.Services.AddSingleton(typeof(INewsService), typeof(NewsService));
builder.Services.AddSingleton(typeof(IVideoService), typeof(VideoService));
builder.Services.AddTransient(typeof(IUploadService), typeof(UploadService));
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
