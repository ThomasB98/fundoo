using BusinessLayer.Authorization;
using BusinessLayer.Service;
using BusinessLayer.ServiceImpl;
using DataLayer.Constants.DBConnection;
using DataLayer.Interfaces;
using DataLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModelLayer.Model.Entity;
using System.Text;
using ModelLayer.Model.Enum;
using DataLayer.Utilities.Profiles;
using DataLayer.Utilities.Token;
using DataLayer.Utilities.Emial;
using DataLayer.Utilities.Redis;

namespace fundoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddDbContext<DataContext>(
                   options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           );
            // Add services to the container.

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                //Role Base policy
                //options.AddPolicy("RequireOwnerRole", policy => policy.RequireRole("OWNER"));
                //options.AddPolicy("RequireEditirRole", policy => policy.RequireRole("EDITOR"));
                //options.AddPolicy("RequireViewerRole", policy => policy.RequireRole("VIEWER"));

                //Permission-Based policy
                options.AddPolicy("RequireEditPermission", policy =>
                    policy.Requirements.Add(new PermissionRequirement(Permission.EditNote)));

                options.AddPolicy("RequireCreatePermission", policy =>
                    policy.Requirements.Add(new PermissionRequirement(Permission.CreateNote)));

                options.AddPolicy("RequireDeletePermission", policy =>
                    policy.Requirements.Add(new PermissionRequirement(Permission.DeleteNote)));

                options.AddPolicy("RequireAddrPermission", policy =>
                    policy.Requirements.Add(new PermissionRequirement(Permission.AddCollaborator)));

                options.AddPolicy("RequireRemovePermission", policy =>
                    policy.Requirements.Add(new PermissionRequirement(Permission.RemoveCollaborator)));

                options.AddPolicy("RequireViewPermission", policy =>
                    policy.Requirements.Add(new PermissionRequirement(Permission.ViewNote)));

            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(UserProfile), typeof(NoteProfile), typeof(LoginProfile));
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddScoped<IUser, UserDL>();
            builder.Services.AddScoped<INote, NoteDL>();
            builder.Services.AddScoped<IAuth, AuthDL>();
            builder.Services.AddScoped<IJwtToken, JwtToken>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IUserService, UserBL>();
            builder.Services.AddScoped<IAuthService,AuthBL>();
            builder.Services.AddScoped<INoteService,NoteBL>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            builder.Services.AddScoped<ICollaboration, CollaborationDL>();
            builder.Services.AddTransient<ICollaborationService, CollaborationBL>();
            builder.Services.AddScoped<ICache, CacheDL>();
            builder.Services.AddScoped<ILabel,LableDL>();
            builder.Services.AddScoped<ILabelService, lableBL>();

            builder.Services.AddSingleton<RedisCacheService>(provider =>
                    new RedisCacheService(builder.Configuration["Redis:ConnectionString"])
            );


            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
        }
    }
}
