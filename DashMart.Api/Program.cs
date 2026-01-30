
using DashMart.Api.Exception;
using DashMart.Application.CurrentUserService;
using DashMart.Application.Registrar;
using DashMart.Infrastructure.Registrar;
using Microsoft.OpenApi.Models;

namespace DashMart.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("DashMartCorsPolicy", policy =>
            {
                policy
                    .WithOrigins(
                        "https://localhost:7004",
                        "http://localhost:5087"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // To take current user info in infrastructure
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services.AddSwaggerGen(options =>
        {
            // ===============================
            // 1) Define the JWT Bearer security scheme
            // ===============================
            //
            // This tells Swagger that our API uses JWT Bearer authentication
            // through the HTTP Authorization header.
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                // The name of the HTTP header where the token will be sent.
                Name = "Authorization",


                // Indicates this is an HTTP authentication scheme.
                Type = SecuritySchemeType.Http,


                // Specifies the authentication scheme name.
                // Must be exactly "Bearer" for JWT Bearer tokens.
                Scheme = "Bearer",


                // Optional metadata to describe the token format.
                BearerFormat = "JWT",


                // Specifies that the token is sent in the request header.
                In = ParameterLocation.Header,


                // Text shown in Swagger UI to guide the user.
                Description = "Enter: Bearer {your JWT token}"
            });


            // ===============================
            // 2) Require the Bearer scheme for secured endpoints
            // ===============================
            //
            // This tells Swagger that endpoints protected by [Authorize]
            // require the Bearer token defined above.
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                // Reference the previously defined "Bearer" security scheme.
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },


            // No scopes are required for JWT Bearer authentication.
            // This array is empty because JWT does not use OAuth scopes here.
            new string[] {}
        }
    });
        });


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler();
       

        app.UseHttpsRedirection();


        app.UseCors("DashMartCorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
