using LogisticApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Configuration;
using Microsoft.CodeAnalysis.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//configuration manager
IConfiguration Configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

//logger

Log.Logger = new LoggerConfiguration()
	.CreateLogger();

//Log.Logger = new LoggerConfiguration()
//	.ReadFrom.Configuration(Configuration)
//	.CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<UzserLojistikContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConn")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
		options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
		{
			Name = "Authorization",
			Type = SecuritySchemeType.ApiKey,
			Scheme = "Bearer",
			BearerFormat = "JWT",
			In = ParameterLocation.Header,
			Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
		});
		options.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
				{
			   new OpenApiSecurityScheme
				 {
					 Reference = new OpenApiReference
					 {
						 Type = ReferenceType.SecurityScheme,
						 Id = "Bearer"
					 }
				 },
				 new string[] {}
				 }
		 });
     }

    );

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
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Configuration["Token:Issuer"],
        ValidAudience = Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"])),
    };
});



try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogisticApi"));
	}
	if (app.Environment.IsProduction())
	{
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/LojistikApi/swagger/v1/swagger.json", "LogisticApi"));
	}


	// To deploy on IIS
	


	app.UseHttpsRedirection();

	app.UseAuthentication();

	app.UseAuthorization();

	app.MapControllers();

	Log.Information("Lojistik API started");

	app.Run();

}
catch (Exception e)
{
	Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}
