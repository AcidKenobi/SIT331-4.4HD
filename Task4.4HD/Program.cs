using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using robot_controller_api.Persistence;
using task3._2.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options => { options.SwaggerDoc("v1", new OpenApiInfo { 
    Title = "Robot Controller API", 
    Description = "New backend service that provides resources for the Moon robot simulator.", 
    Contact = new OpenApiContact { 
            Name = "Clint Bidner",
            Email = "s220534168@deakin.edu.au"
        }, 
    }); 

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); 
}); 

builder.Services.AddOpenApi();

//4.1P
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
//builder.Services.AddScoped<IMapDataAccess, MapADO>();

//4.2C
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
//builder.Services.AddScoped<IMapDataAccess, MapRepository>();

//4.3D
//builder.Services.AddDbContext<RobotContext>(options =>
builder.Services.AddDbContext<RobotContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .LogTo(Console.Write).EnableSensitiveDataLogging());
    

//4.4HD 
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
builder.Services.AddScoped<IMapDataAccess, MapEF>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.MapOpenApi();
    app.UseSwagger();
    //app.UseSwaggerUi(options => { options.DocumentPath = "/openapi/v1.json"; });
    app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-muted.css"));
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();