using ChatAppServer.Data;
using ChatAppServer.Data.Repositories;
using ChatAppServer.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Net.Sockets;
using System.Threading.Channels;
using ChatAppServer.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;


namespace ChatAppServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            

            SetUpBuilder(builder);

            var app = builder.Build();

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true // Allow serving unknown file types
            });
            


            app.UseCors("AllowAll"); // to delete 
            app.MapHub<ChatManager>("/chat");

            


            app.UseAuthentication();
            app.UseAuthorization();



            
            app.Use(async (context, next) =>
            {
                // If the request path is "/"
                if (context.Request.Path.Equals("/", StringComparison.OrdinalIgnoreCase))
                {
                    // Redirect to "/home"
                    context.Response.Redirect("/home");
                    return; 
                }

                await next(); 
            });

            //app.MapDefaultControllerRoute();
            app.MapControllers();



            
            app.Run();
        }

        public static void SetUpBuilder(WebApplicationBuilder builder)
        {
            //builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR(options =>
            {
                
            }); // Add SignalR services, for socket comms
            

            // CORS: Allow all origins, to delete
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("authToken");
                });
            });


            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IMessagesRepo, MessagesRepo>();
            builder.Services.AddScoped<IDirectChatRepo, DirectChatRepo>();
            builder.Services.AddScoped<IGroupChatRepo, GroupChatRepo>();
            builder.Services.AddScoped<CentrallizedRepo>();
            builder.Services.AddScoped<Validator>();


            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();


            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtHelper>();
            jwtSettings!.SetValidationParameters();

            builder.Services.AddSingleton(jwtSettings!);


            var usersMsgQueue = new MessageQueue();
            var usersDcToken = new ConcurrentDictionary<string, CancellationTokenSource>();
            
            builder.Services.AddSingleton(usersMsgQueue);
            builder.Services.AddSingleton(usersDcToken);
            

            var key = Encoding.ASCII.GetBytes(jwtSettings!.Key!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience
                };

                // Read token from cookie
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {

                        if (context.Request.Headers.TryGetValue("Authorization", out var value))
                        {
                            var authHeader = value.ToString();
                            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = authHeader.Substring("Bearer ".Length).Trim();
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddDbContext<ChatAppDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                        .UseLazyLoadingProxies()
                        );


        }
    }
}
