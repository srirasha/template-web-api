using Application._Common.Interfaces;
using Infrastructure;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Web.MyAPI.Middlewares;
using Web.MyAPI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(AddSwaggerDocumentation);
var app = builder.Build();

app.UseMiddleware<ExceptionsHandlerMiddleware>();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //database initialization
    if (!app.Configuration.GetValue<bool>("UseInMemoryDatabase"))
    {
        using IServiceScope scope = app.Services.CreateScope();
        ApplicationDbContextInitialiser dbInitialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await dbInitialiser.Initialise();
    }
}

app.MapControllers();

app.Run();

static void AddSwaggerDocumentation(SwaggerGenOptions o)
{
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}