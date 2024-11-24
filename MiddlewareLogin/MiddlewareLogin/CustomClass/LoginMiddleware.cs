namespace MiddlewareLogin.CustomClass
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;
        private string correctEmail = "admin@example.com";
        private string correctPassword = "admin1234";

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "POST")
            {
                bool isContainsEmail = httpContext.Request.Query.ContainsKey("email");
                bool isContainsPassword = httpContext.Request.Query.ContainsKey("password");

                bool isLoginInputComplete = isContainsEmail && isContainsPassword;

                if (isLoginInputComplete) 
                {
                    string email = httpContext.Request.Query["email"];
                    string password = httpContext.Request.Query["password"];

                    bool isEmailMatch = email == correctEmail;
                    bool isPasswordMatch = password == correctPassword;

                    if (isEmailMatch && isPasswordMatch)
                    {
                        await httpContext.Response.WriteAsync("Successful login\n");
                    } 
                    else
                    {
                        httpContext.Response.StatusCode = 400;
                        await httpContext.Response.WriteAsync("Invalid login\n");
                    }
                } 
                else
                {
                    httpContext.Response.StatusCode = 400;
                    if (!isContainsEmail)
                        await httpContext.Response.WriteAsync("Invalid input for 'email'\n");
                    if (!isContainsPassword)
                        await httpContext.Response.WriteAsync("Invalid input for 'password'\n");
                }
            } else
            {
                await httpContext.Response.WriteAsync("No response\n");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
