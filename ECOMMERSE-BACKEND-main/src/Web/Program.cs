using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Service;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Web.Controllers;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("EcommerceApiBearerAuth", new OpenApiSecurityScheme() 
    {
        Type = SecuritySchemeType.Http,  
        Scheme = "Bearer", 
        Description = "Acá pegar el token generado al loguearse."  
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,  
                    Id = "EcommerceApiBearerAuth" } 
                }, new List<string>() } 
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); 
    setupAction.IncludeXmlComments(xmlPath); 

});


string connectionString = builder.Configuration["ConnectionStrings:DBConnectionString"]!;

var connection = new SqliteConnection(connectionString);
connection.Open();

using (var command = connection.CreateCommand())
{
    command.CommandText = "PRAGMA journal_mode = DELETE;";
    command.ExecuteNonQuery();
}

builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseSqlite(connection));


#region Repositories
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderProductRepository, OrderProductRepository>();
builder.Services.AddScoped<IOrderNotificationRepository, OrderNotificationRepository>();
builder.Services.AddScoped<IRepositoryBase<Product>, EfRepository<Product>>();
builder.Services.AddScoped<IRepositoryBase<Order>, EfRepository<Order>>();
builder.Services.AddScoped<IRepositoryBase<Product>, EfRepository<Product>>();
builder.Services.AddScoped<IRepositoryBase<OrderNotification>, EfRepository<OrderNotification>>();
#endregion

#region Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISendEmailService, SendEmailService>();
builder.Services.AddScoped<IEmailService, EmailService>();
#endregion



builder.Services.Configure<AutenticacionServiceOptions>(
    builder.Configuration.GetSection(AutenticacionServiceOptions.AutenticacionService));


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options => 
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true, 
        ValidateAudience = true, 
        ValidateIssuerSigningKey = true, 
        ValidIssuer = builder.Configuration["AutenticacionService:Issuer"], 
        ValidAudience = builder.Configuration["AutenticacionService:Audience"], 
        IssuerSigningKey = new SymmetricSecurityKey( 
            Encoding.ASCII.GetBytes(builder.Configuration["AutenticacionService:SecretForKey"])) 
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole(UserType.Admin.ToString()));
    options.AddPolicy("SellerPolicy", policy => policy.RequireRole(UserType.Seller.ToString()));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole(UserType.Customer.ToString()));
    options.AddPolicy("AdminOrSellerPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole(UserType.Admin.ToString()) ||
            context.User.IsInRole(UserType.Seller.ToString())));
    options.AddPolicy("CustomerOrSellerPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole(UserType.Customer.ToString()) ||
            context.User.IsInRole(UserType.Seller.ToString())));
    options.AddPolicy("EveryoneExceptGuests", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole(UserType.Customer.ToString()) ||
            context.User.IsInRole(UserType.Seller.ToString()) ||
            context.User.IsInRole(UserType.Admin.ToString())));
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
