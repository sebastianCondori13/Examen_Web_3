using System.Text;
using System.Security.Cryptography;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SportHub.Api.Data;
using SportHub.Api.Services;

var builder=WebApplication.CreateBuilder(args);
Directory.CreateDirectory(".local");
// Clave aleatoria de desarrollo. No se publica una clave JWT reutilizable en Git.
if(string.IsNullOrWhiteSpace(builder.Configuration["Jwt:Key"]))
{
    if(!builder.Environment.IsDevelopment())throw new InvalidOperationException("Configura Jwt__Key para este entorno.");
    var path=Path.Combine(".local","jwt.key");
    if(!File.Exists(path))File.WriteAllText(path,Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)));
    builder.Configuration["Jwt:Key"]=File.ReadAllText(path).Trim();
}
if(Encoding.UTF8.GetByteCount(builder.Configuration["Jwt:Key"]!)<32)throw new InvalidOperationException("Jwt__Key necesita al menos 32 bytes.");
builder.Services.AddDbContext<AppDbContext>(o=>o.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<TokenService>();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddCors(o=>o.AddDefaultPolicy(p=>p.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()??[]).AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o=>o.TokenValidationParameters=new TokenValidationParameters{
    ValidateIssuer=true,ValidateAudience=true,ValidateLifetime=true,ValidateIssuerSigningKey=true,
    ValidIssuer=builder.Configuration["Jwt:Issuer"],ValidAudience=builder.Configuration["Jwt:Audience"],
    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),ClockSkew=TimeSpan.FromSeconds(15)
});
builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(o=>{
    o.RejectionStatusCode=429;
    o.AddPolicy("auth",context=>RateLimitPartition.GetFixedWindowLimiter(context.Connection.RemoteIpAddress?.ToString()??"local",_=>new FixedWindowRateLimiterOptions{PermitLimit=30,Window=TimeSpan.FromMinutes(1),QueueLimit=0}));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o=>{
    o.SwaggerDoc("v1",new OpenApiInfo{Title="SportHub API",Version="v1",Description="Tienda deportiva · API para React y React Native"});
    o.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme{Type=SecuritySchemeType.Http,Scheme="bearer",BearerFormat="JWT",Description="Pega solamente el token del login."});
    o.AddSecurityRequirement(new OpenApiSecurityRequirement{{new OpenApiSecurityScheme{Reference=new OpenApiReference{Type=ReferenceType.SecurityScheme,Id="Bearer"}},Array.Empty<string>()}});
});
var app=builder.Build();
app.UseExceptionHandler();
// Mensajes conocidos sin divulgar trazas, SQL ni datos internos a los clientes.
app.Use(async(ctx,next)=>{
    try {await next();}
    catch(DbUpdateException){ctx.Response.StatusCode=409;await ctx.Response.WriteAsJsonAsync(new{message="El registro está duplicado o está relacionado con otros datos."});}
    catch(SqliteException){ctx.Response.StatusCode=409;await ctx.Response.WriteAsJsonAsync(new{message="La base está ocupada. Reintenta la operación."});}
});
app.UseCors();app.UseRateLimiter();app.UseAuthentication();app.UseAuthorization();
if(app.Environment.IsDevelopment()){app.UseSwagger();app.UseSwaggerUI();}
app.MapGet("/health",()=>Results.Ok(new{status="ok",service="SportHub.Api"}));
app.MapControllers();
using(var scope=app.Services.CreateScope())
{
    var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    if(app.Environment.IsDevelopment())await SeedData.Initialize(db);
    if(args.Contains("--export-schema")){await File.WriteAllTextAsync("schema.sql",db.Database.GenerateCreateScript());return;}
}
app.Run();
