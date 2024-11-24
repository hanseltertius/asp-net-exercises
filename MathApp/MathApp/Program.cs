var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    if (context.Request.Method == "GET")
    {
        IQueryCollection query = context.Request.Query;
        List<string> errorMessages = [];

        if (!query.ContainsKey("firstNumber")) {
            context.Response.StatusCode = 400;
            errorMessages.Add("Invalid input for 'firstNumber'");
        }

        if (!query.ContainsKey("secondNumber")) {
            context.Response.StatusCode = 400;
            errorMessages.Add("Invalid input for 'secondNumber'");
        }

        if (query.ContainsKey("operation"))
        {
            List<string> validOperations = new List<string> { "add", "subtract", "multiply", "division", "modulus" };
            string operation = context.Request.Query["operation"];
            if (!validOperations.Contains(operation))
            {
                context.Response.StatusCode = 400;
                errorMessages.Add("Invalid input for 'operation'");
            }
        }
        else {
            context.Response.StatusCode = 400;
            errorMessages.Add("Invalid input for 'operation'");
        }

        if (context.Response.StatusCode == 200)
        {
            int result = 0;

            string operation = context.Request.Query["operation"];
            int firstNumber = Int32.Parse(context.Request.Query["firstNumber"]);
            int secondNumber = Int32.Parse(context.Request.Query["secondNumber"]);

            switch (operation)
            {
                case "add":
                    result = firstNumber + secondNumber;
                    break;
                case "subtract":
                    result = firstNumber - secondNumber;
                    break;
                case "multiply":
                    result = firstNumber * secondNumber;
                    break;
                case "division":
                    result = secondNumber != 0 ? firstNumber / secondNumber : 0;
                    break;
                case "modulus":
                    result = secondNumber != 0 ? firstNumber % secondNumber : 0;
                    break;
            }

            await context.Response.WriteAsync($"{result}");
        } 
        else if (context.Response.StatusCode == 400)
        {
            foreach (var (index, errorMessage) in errorMessages.Select((errorMessage, index) => (index, errorMessage)))
            {
                await context.Response.WriteAsync(errorMessage);
                if (index != errorMessages.Count - 1)
                    await context.Response.WriteAsync("\n");
            }
        }
    }
});

app.Run();
