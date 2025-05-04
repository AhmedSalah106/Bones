
using Bones_App.CustomMiddleWare;
using Bones_App.Helpers;
using Bones_App.Hubs;
using Bones_App.Models;
using Bones_App.Repositories.Implementation;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Implementation;
using Bones_App.Services.Interfaces;
using Bones_App.Services.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

namespace Bones_App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddHangfire(x => x
             .UseSqlServerStorage(builder.Configuration.GetConnectionString("cs")));

            builder.Services.AddHangfireServer();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "3dma",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
            });


            builder.Services.AddMediatR(option =>
            {
                option.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPatientService, PatientService>();

            builder.Services.AddScoped<ISpecialistReposiotry, SpecialistRepository>();
            builder.Services.AddScoped<ISpecialistService, SpecialistService>();

            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();

            builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
            builder.Services.AddScoped<IpaymentTransactionService, PaymentTransactionService>();

            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddScoped<IModelIntegrationService,ModelIntegrationService>();

            builder.Services.AddScoped<ISpecialistRateRepository, SpecialistRateRepository>();
            builder.Services.AddScoped<ISpecialistRateService, SpecialistRateService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<PaymentService>();
            builder.Services.AddScoped<SpecialistJobs>();
            builder.Services.AddScoped<PatientJobs>();
            builder.Services.AddScoped<EmailJobs>();
            builder.Services.AddHttpClient<PaymobService>();

            builder.Services.AddScoped<EmailService>();

            builder.Services.AddDbContext<BonesContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;

                option.Password.RequiredLength = 4;
                option.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<BonesContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // 100MB limit, adjust if needed
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidISS"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAud"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecritKey"]))
                };
            });

            builder.Services.AddRateLimiter(option =>
            {
                option.AddFixedWindowLimiter("Fixed", opt =>
                {
                    opt.PermitLimit = 5;
                    opt.Window = TimeSpan.FromSeconds(1);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;
                });


                option.RejectionStatusCode = 429;
            });

            builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });


            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleWare>();

            app.UseRateLimiter();
            app.MapGet("/limited-endpoint", () => "Hello with rate limit!")
                .RequireRateLimiting("Fixed");


            // Configure the HTTP request pipeline.


            // Always enable Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bones API V1");
                c.RoutePrefix = "swagger"; // This makes it accessible at /swagger
            });


            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "Admin", "Patient", "Specialist" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var context = scope.ServiceProvider.GetRequiredService<BonesContext>();
                DbInitializer.SeedAdmin(unitOfWork, context);
            }

            app.UseStaticFiles();
            app.UseHangfireDashboard("/Dashboard");
            app.UseHangfireServer();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/ChatH");

            app.MapControllers();

            app.Run();
        }
    }
}