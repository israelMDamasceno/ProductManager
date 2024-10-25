using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Contexts;
using ProductManager.Infrastructure.Repositories;
using System.Reflection;
using System.Text;

namespace ProductManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblies(typeof(CreatedProductCommandHandler).Assembly));
            builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblies(typeof(UpdatedProductCommandHandler).Assembly));
            builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblies(typeof(DeletedProductCommandHandler).Assembly));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();
            var key = Encoding.ASCII.GetBytes(Key.Secret);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                     }
                });


            });
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });



            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRepository<Product>, Repository>();
            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
