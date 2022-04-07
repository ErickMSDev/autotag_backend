using AutoMapper;
using AutoTagBackEnd;
using AutoTagBackEnd.AppModels;
using AutoTagBackEnd.Helpers;
using AutoTagBackEnd.Models;
using AutoTagBackEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();

//string myCors = "MyCors";

builder.Services.AddCors(options =>
{
    //options.AddPolicy(myCors, builder =>
    //{
    //    builder.WithOrigins("http://localhost:3000");
    //});
    options.AddDefaultPolicy(builderCors =>
    {
        builderCors.WithOrigins(
            "http://localhost:3000",
            "http://localhost:8080",
            "https://viasimple.cl",
            "https://www.viasimple.cl",
            "https://app.viasimple.cl")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<AzureAd>(builder.Configuration.GetSection("AzureAd"));
builder.Services.Configure<PowerBI>(builder.Configuration.GetSection("PowerBI"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(AadService));
builder.Services.AddScoped(typeof(PbiEmbedService));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<AutoTagContext>(
    dbContextOptions => dbContextOptions
        .UseSqlServer("name=Production")
        // The following three options help with debugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();