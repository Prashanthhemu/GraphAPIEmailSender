using GraphAPIMailSender.Interfaces;
using GraphAPIMailSender.Models;
using GraphAPIMailSender.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<GraphAPIKeys>(builder.Configuration.GetSection("GraphAPIKeys"));
builder.Services.AddScoped<IMailSender, MailSender>();
builder.Services.AddControllers();

var app = builder.Build();
var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
