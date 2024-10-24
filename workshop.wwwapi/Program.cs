using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CarContext>(opt => opt.UseInMemoryDatabase("CarList"));
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7196") // Add your frontend's origin here
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Car API", Version = "v1" });
});

var app = builder.Build();

// Seed the database with initial data
SeedDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car API v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS middleware
app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();

void SeedDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CarContext>();

        // Add initial data to the in-memory database
        context.Cars.AddRange(
            new Car { Make = "Toyota", Model = "Corolla", Year = 2020 },
            new Car { Make = "Honda", Model = "Civic", Year = 2019 },
            new Car { Make = "Ford", Model = "Mustang", Year = 2021 }
        );

        context.SaveChanges();
    }
}
