using BD4.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));
builder.Services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("secretType", typeof(SecreteType)));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Map("/", () => "Plant Page");
app.Map("/plant", () => new Plant(1, "rose", "cute", "red", 500));
app.Map("/plant/{id}/{name}/{description}/{type}/{price}", (int id, string name, string description, string type, int price)
    => $"Plant id: {id}\nPlant name: {name}\nPlant description: {description}\nPlant type: {type}\nPlant price: {price}");

app.Map("/plant/{id}/{code:secretcode(123)}", (int id, string code) => $"Plant id: {id}");

/*app.Map(
    "{controller=Plant}/{action=Get}/{id?}",
    (string controller, string action, string? id) =>
        $"Controller: {controller} \nAction: {action} \nId: {id}"
);*/

app.Map(
    "/plantPrice/{name:alpha:minlength(2)}/{price:int:range(1, 110)}",
    (string name, int price) => $"Plant name: {name}\nPlant price: {price}"
);

app.Map("/plant/{id}/{code:secretcode(123)}", (int id, string code) => $"Plant id: {id}");
app.Map("/type/{type:secretType}", (string type) => $"Type: {type}");

app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));

app.Run();

public class SecretCodeConstraint : IRouteConstraint
{
    string secretCode;    // допустимый код
    public SecretCodeConstraint(string secretCode)
    {
        this.secretCode = secretCode;
    }

    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        return values[routeKey]?.ToString() == secretCode;
    }
}

public class SecreteType : IRouteConstraint
{
    List<string> types = ["rose", "кактус", "вьюнок"];
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue("type", out object value) && value is string typeString)
        {
            
                return types.Contains(typeString);
            
        }

        return false;
    }
}

