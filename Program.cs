using SWP391.Project.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServices();

WebApplication app = builder.Build();

app.UseMiddlewares();

app.Run();
