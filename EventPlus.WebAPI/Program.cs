using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Repositories;
using EventPlus.WebAPI.Repository;
using EventPlus.WebAPI.Utils;
using EventPlus.WebAPI.Workers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do EFCore - Banco de dados
builder.Services.AddDbContext<EventContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração de Controllers e JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Injeção de dependência
builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoEvento, TipoEventoRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IInstituicao, InstituicaoRepository>();
builder.Services.AddScoped<IEvento, EventoRepository>();
builder.Services.AddScoped<IInscricao, InscricaoRepository>();
builder.Services.AddScoped<IComentario, ComentarioRepository>();

builder.Services.AddTransient<IEnviarEmailInterface, EnviarEmail>();
builder.Services.AddHostedService<EmailAutomaticoWorker>();

// Configuração de Autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "EventPlus.WebAPI",
        ValidateAudience = true,
        ValidAudience = "EventPlus.WebAPI",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "Jwt:Key"))
    };
});

builder.Services.AddAuthorization();

// --- Sightengine (plano Free, sem cartão) ---
// builder.Services.Configure<SightengineSettings>(builder.Configuration.GetSection("Sightengine"));
// builder.Services.AddHttpClient<IModerationService, SightengineModerationService>(client =>
// {
//     client.BaseAddress = new Uri("https://api.sightengine.com/1.0/");
// });

// ==========================================
// Swagger PURO (Sem configuração de JWT)
// ==========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EventPlus API",
        Version = "v1",
        Description = "API do sistema EventPlus"
    });
});

builder.Services.AddCors(options =>
 {
     options.AddPolicy("AllowAll",
         policy => policy
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
 });




var app = builder.Build();

app.UseCors("AllowAll");
// ==========================================
// Middlewares e Pipeline HTTP
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EventPlus API v1");
        options.RoutePrefix = string.Empty; // Abre direto no http://localhost:<porta>/
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();