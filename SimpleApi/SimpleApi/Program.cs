// This code will initialize a new instance of the WebApplicationBuilder class with preconfigured defaults.
var builder = WebApplication.CreateBuilder(args);

// This code will add services to the container for dependency injection and options configuration.
// This code line will add API explorer services necessary for Swagger integration.
builder.Services.AddEndpointsApiExplorer();
// This code line will register Swagger generator services to enable the generation of Swagger documents for the API.
builder.Services.AddSwaggerGen();

// This code will build the application, compiling the services and configurations into a runnable application.
var app = builder.Build();

// This code will configure the HTTP request pipeline.
// This code line will check if the application is running in the Development environment.
if (app.Environment.IsDevelopment())
{
    // This code line will add middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // This code line will add middleware to serve the Swagger UI, a web-based UI for testing the API.
    app.UseSwaggerUI();
}

// This code line will add middleware to redirect HTTP requests to HTTPS, enhancing security.
app.UseHttpsRedirection();

// This code will define a GET endpoint at "/Customers" that, when hit, calls DatabaseManager.GetCustomers to fetch customer data.
app.MapGet("/Customers", () =>
{
    // This code line will return the result of the GetCustomers method, fetching customer data from the database.
    return DatabaseManager.GetCustomers();
})
// This code line will assign a name to this endpoint, useful for generating URLs within the application.
.WithName("GetCustomers");

// This code will define a POST endpoint at "/Customers" to receive a Customer object and create a new customer in the database.
app.MapPost("/Customers", (Customer customer) =>
{
    // This code line will save the new customer to the database using CreateCustomer method.
    DatabaseManager.CreateCustomer(customer);
    // This code line will return a 201 Created response with a location header for the newly created customer resource.
    return Results.Created($"/Customers/{customer.CustomerId}", customer);
})
// This code line will assign a name to this endpoint as well, for URL generation purposes.
.WithName("CreateCustomer");

// This code line will start running the web application on the configured port, making it accessible to clients.
app.Run();
