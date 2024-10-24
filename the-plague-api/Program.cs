using The_Plague_Api.Data.Entities;
using The_Plague_Api.Extension;
using The_Plague_Api.Repositories;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Settings;
using Microsoft.OpenApi.Models; // Import this namespace for Swagger

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
ConfigureServices(builder);

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
ConfigureMiddleware(app);

// Run the application
app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
  // Add controllers
  builder.Services.AddControllers();

  // Add MongoDB settings
  builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
  builder.Services.AddMongoDbConnection();

  // Add Dependency Injection for Repositories
  builder.Services.AddSingleton<IProductRepository, ProductRepository>();
  builder.Services.AddSingleton<IDiscountRepository, DiscountRepository>();

  // Add Dependency Injection for Services
  builder.Services.AddScoped<IProductService, ProductService>();
  builder.Services.AddScoped<IDiscountService, DiscountService>();

  // Add CORS services
  builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowLocalhost", builder =>
      {
        builder.WithOrigins("http://localhost:3000") // Allow your Next.js app
                 .AllowAnyMethod()
                 .AllowAnyHeader();
      });
  });

  // Add Swagger services
  builder.Services.AddSwaggerGen(c =>
  {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "The Plague API", Version = "v1" });
  });
}

void ConfigureMiddleware(WebApplication app)
{
  // Uncomment the following lines to enable Swagger in development
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "The Plague API V1");
    });
  }

  app.UseHttpsRedirection();
  app.UseCors("AllowLocalhost");
  app.UseAuthorization();
  app.MapControllers();
}
