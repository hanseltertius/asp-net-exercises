using MiddlewareLogin.CustomClass;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseLoginMiddleware();

app.Run();
