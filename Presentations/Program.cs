using DataAccesses.Models;
using DataAccesses.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessLogics.Repositories;
using BusinessLogics.RepositoriesImpl;
using DataAccesses.Seeds;
using Presentations.Hubs;
using Microsoft.OpenApi.Models;
using Presentations.Middlewares;
using Microsoft.AspNetCore.SignalR;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Presentations.AuthorizationHandler.RequiredPermission;
using Presentations.AuthorizationHandler.AllowedIds;
using Presentations.AuthorizationHandler.IsUserActive;
using Presentations.AuthorizationHandler.CheckUserActive;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Handle case insensitivity
});
builder.Services.AddDbContext<FakeDiscordContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSignalR();
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


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    var permissions = new[]
    {
        Permissions.CAN_MANAGE_CHANNELS,
        Permissions.CAN_MANAGE_ROLES,
        Permissions.CAN_CREATE_INVITES,
        Permissions.CAN_SEND_MESSAGES,
        Permissions.CAN_MANAGE_MESSAGES,
        Permissions.CAN_EDIT_GROUPCHAT,
        Permissions.CAN_MANAGE_MEMBERS,
    };

    foreach (var permission in permissions)
    {
        options.AddPolicy(permission, policy =>
            policy.Requirements.Add(new RequiredPermissionRequirement(permission)));
    }

    options.AddPolicy("VIEW_MESSAGES", policy =>
    policy.Requirements.Add(new PrivateChannelsAllowersRequirement()));

    options.AddPolicy("CHECK_ACTIVE", policy =>
    policy.Requirements.Add(new CheckUserActiveRequirement()));
});

builder.Services.AddTransient<IAuthorizationHandler, RequiredPermissionHandler>();
builder.Services.AddTransient<IAuthorizationHandler, PrivateChannelsAllowersHandler>();
builder.Services.AddTransient<IAuthorizationHandler, CheckUserActiveHandler>();
builder.Services.AddHttpContextAccessor();
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddAutoMapper(typeof(UserProfile));
// DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupChatRepository, GroupChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<ISuperAdminRepository, SuperAdminRepository>();
builder.Services.AddTransient<IEmailRepository, EmailRepository>();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.AddSingleton<UserTracker>();
builder.Services.AddScoped<RandomStringGenerator>();
var app = builder.Build();
// add seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FakeDiscordContext>();
    var seeder = new DataSeeder(context);
    seeder.SeedRoles();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.UseMiddleware<TokenVerifyMiddleware>();
app.UseMiddleware<ExceptionsHandlingMiddleware>();
app.MapHub<UserHub>("/userHub");
app.MapHub<GroupChatHub>("/groupChatHub");
app.MapHub<ChannelHub>("/channelHub");
app.MapHub<AdminHub>("/adminHub");
app.MapControllers();
app.Run();
