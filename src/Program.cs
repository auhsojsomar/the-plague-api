using The_Plague_Api.Extensions;
using The_Plague_Api.Repositories;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Settings;
using The_Plague_Api.Data.MappingPorfiles;
using Microsoft.OpenApi.Models;
using The_Plague_Api.Extension;

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
  builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
  });


  builder.Services.AddMvc(options =>
  {
    // Prevents ASP.NET Core from removing the "Async" suffix from action names.
    // This ensures that routes referencing methods with "Async" in their names 
    // (e.g., GetUserByIdAsync) are correctly matched. 
    // Without this, you might encounter routing issues if route names and method names don’t align.
    options.SuppressAsyncSuffixInActionNames = false;
  });

  // Add JWT Authentication
  builder.Services.AddJwtAuthentication(builder.Configuration);

  // Register AutoMapper
  builder.Services.AddAutoMapper(typeof(ProductProfile));
  builder.Services.AddAutoMapper(typeof(UserProfile));
  builder.Services.AddAutoMapper(typeof(AdminProfile));
  builder.Services.AddAutoMapper(typeof(OrdeProfile));

  // Add MongoDB settings
  builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
  builder.Services.AddMongoDbConnection();

  // Register repositories
  builder.Services.AddSingleton<IProductRepository, ProductRepository>();
  builder.Services.AddSingleton<IDiscountRepository, DiscountRepository>();
  builder.Services.AddSingleton<IUserRepository, UserRepository>();
  builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
  builder.Services.AddSingleton<IOrderStatusRepository, OrderStatusRepository>();
  builder.Services.AddSingleton<IPaymentMethodRepository, PaymentMethodRepository>();
  builder.Services.AddSingleton<IPaymentStatusRepository, PaymentStatusRepository>();
  builder.Services.AddSingleton<ICartRepository, CartRepository>();
  builder.Services.AddSingleton<IShippingFeeRepository, ShippingFeeRepository>();
  builder.Services.AddSingleton<IAdminRepository, AdminRepository>();

  // Register services
  builder.Services.AddSingleton<KeyGeneratorService>();
  builder.Services.AddScoped<IProductService, ProductService>();
  builder.Services.AddScoped<IDiscountService, DiscountService>();
  builder.Services.AddScoped<IUserService, UserService>();
  builder.Services.AddScoped<IOrderService, OrderService>();
  builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();
  builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
  builder.Services.AddScoped<IPaymentStatusService, PaymentStatusService>();
  builder.Services.AddScoped<ICartService, CartService>();
  builder.Services.AddScoped<IShippingFeeService, ShippingFeeService>();
  builder.Services.AddScoped<IAdminService, AdminService>();

  // Add CORS
  builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowLocalhost", builder =>
      {
        builder.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
      });

    options.AddPolicy("AllowProduction", builder =>
      {
        builder.WithOrigins("https://the-plague-clothing-dev.netlify.app",
                            "https://the-plague.up.railway.app")
              .AllowAnyMethod()
              .AllowAnyHeader();
      });
    // Add internal Railway hostname for the frontend service
    options.AddPolicy("AllowFrontendInternal", builder =>
      {
        builder.WithOrigins("http://the_plague_client:3000") // Internal hostname for the frontend service
              .AllowAnyMethod()
              .AllowAnyHeader();
      });
  });

  // Add Swagger
  builder.Services.AddSwaggerGen(c =>
  {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "The Plague API", Version = "v1" });
  });
}

void ConfigureMiddleware(WebApplication app)
{
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

  app.UseAuthentication();
  app.UseAuthorization();

  app.MapControllers();
}
