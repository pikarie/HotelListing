using HotelListing.API.Configuration;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Middlewares;
using HotelListing.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
	options.UseSqlServer(
		connectionString,
		b => b.MigrationsAssembly(typeof(HotelListingDbContext).Assembly.FullName)
		);
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(o =>
{
	var accessor = o.GetRequiredService<IHttpContextAccessor>();
	var request = accessor.HttpContext.Request;
	var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
	return new UriService(uri);
});

builder.Services.AddIdentityCore<ApiUser>()
	.AddRoles<IdentityRole>()
	.AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingApi")
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<HotelListingDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", builder => builder
		.AllowAnyHeader()
		.AllowAnyOrigin()
		.AllowAnyMethod()
	);
});

builder.Services.AddApiVersioning(options =>
{
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
	options.ReportApiVersions = true;
	options.ApiVersionReader = ApiVersionReader.Combine(
		new QueryStringApiVersionReader("api-version"),
		new HeaderApiVersionReader("X-Version"),
		new MediaTypeApiVersionReader("ver")
	);
});

builder.Services.AddVersionedApiExplorer(
	options =>
	{
		options.GroupNameFormat = "'v'VVV";
		options.SubstituteApiVersionInUrl = true;
	});

builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

builder.Services.AddAutoMapper(typeof(AutomapperConfig));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero,
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
		ValidAudience = builder.Configuration["JwtSettings:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
	};
});

builder.Services.AddResponseCaching(options =>
{
	options.MaximumBodySize = 1024;
	options.UseCaseSensitivePaths = true;
});

builder.Services.AddControllers()
.AddOData(options => {
	options.Select().Filter().OrderBy();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseResponseCaching();

app.Use(async (context, next) =>
{
	context.Response.GetTypedHeaders().CacheControl =
	new CacheControlHeaderValue()
	{
		Public = true,
		MaxAge = TimeSpan.FromSeconds(1)//5
	};
	context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };

	await next();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
