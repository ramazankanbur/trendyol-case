using TY.Hiring.Fleet.Management.Api.Middlewares;
using TY.Hiring.Fleet.Management.Api.Utils;
using TY.Hiring.Fleet.Management.AppConfig;
using TY.Hiring.Fleet.Management.Data.ORM.EF;
using TY.Hiring.Fleet.Management.Mapper;

var builder = WebApplication.CreateBuilder(args);

ConfigurationService.Configure(builder.Configuration);

builder.Services.AddAppServices();

builder.Services.AddDataLayer();
builder.Services.AddAutoMapper(typeof(EntryPoint));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

var app = builder.Build();

MsSqlDbInitializer.InitializeDb(app);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TY Fleet.Management.Api");
});


app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
