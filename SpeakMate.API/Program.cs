
using SpeakMate.API.Hubs;
using SpeakMate.API.Middlewares;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Core.Repositories;
using SpeakMate.Infrastructure.Data;
using SpeakMate.Infrastructure.Helpers;
using SpeakMate.Infrastructure.Repositories;
using SpeakMate.Infrastructure.Security;
using SpeakMate.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.SymbolStore;
using System.Text;

namespace SpeakMate.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddSignalR();
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("Dev", policy =>
                    policy.WithOrigins("null", "http://localhost", "https://localhost")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
            });

            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<ILikesRepository, LikesRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            // Singleton because the dictionary must live for the app's lifetime
            builder.Services.AddSingleton<IUserTracker, UserTrackerRepositories>();
            builder.Services.AddScoped<LogUserActivity>();
            builder.Services.AddScoped<DiffService>();
            builder.Services.AddSingleton<LanguageDetectionService>();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ITranslationService, TranslationService>();
            builder.Services.AddScoped<ISavedWordRepository, SavedWordRepository>();
            builder.Services.AddScoped<IMessageCorrectionRepository, MessageCorrectionRepository>();
            builder.Services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/Hubs"))
                            context.Token = token;
                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
           

            var app = builder.Build();

           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseCors("Dev");
            //app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<PresenceHub>("Hubs/presence");
            app.MapHub<MessageHub>("/hubs/message");
             



            using var scope=app.Services.CreateScope();
           var services= scope.ServiceProvider;
            try
            {
                Console.WriteLine("be runing");

                var context = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                await context.Database.MigrateAsync();
                await Seed.SeedUsers(userManager);
            }
            catch (Exception ex)
            {
                var logger= services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }
            Console.WriteLine("Afrer runing");
            app.Run();
        }
    }
}
