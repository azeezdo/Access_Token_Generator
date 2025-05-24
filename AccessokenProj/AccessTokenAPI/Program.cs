using System.Text;
using AccessTokenApplication.Services;
using AccessTokenDomain.Entity;
using AccessTokenDomain.Interfaces.IRepositories;
using AccessTokenDomain.Interfaces.IServices;
using AccessTokenInfrastructure.Context;
using AccessTokenInfrastructure.Repositories;
using AccessTokenInfrastructure.UnitofWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AccessTokenDB");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentityCore<User>( opt =>
            {
    opt.User.RequireUniqueEmail = true;
    opt.Tokens.PasswordResetTokenProvider = "AccessTokenProvider";
})
       .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Access Token Generator",
        Version = "v1"
    });
    c.CustomSchemaIds(i => i.FullName);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                }
             });
});

//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//                .AddJwtBearer(o =>
//                {
//                    var Key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
//                    o.SaveToken = true;
//                    o.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuer = false, // on production make it true
//                        ValidateAudience = false, // on production make it true
//                        ValidateLifetime = true,
//                        ValidateIssuerSigningKey = true,
//                        ValidIssuer = configuration["JWT:Issuer"],
//                        ValidAudience = configuration["JWT:Audience"],
//                        IssuerSigningKey = new SymmetricSecurityKey(Key),
//                        ClockSkew = TimeSpan.Zero
//                    };
//                    o.Events = new JwtBearerEvents
//                    {
//                        OnAuthenticationFailed = context =>
//                        {
//                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
//                            {
//                                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
//                            }
//                            return Task.CompletedTask;
//                        }
//                    };
//                });
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

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

app.Run();

