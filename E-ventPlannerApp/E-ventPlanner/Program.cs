using E_ventPlanner.Contexts;
using E_ventPlanner.Services;
using E_ventPlanner.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PlannerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 5;
        options.Password.RequireNonAlphanumeric = false; //Of course these should be true in a working App for better safety.
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase= false;
        options.User.RequireUniqueEmail = true;
    }).AddEntityFrameworkStores<PlannerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IRegisterService, RegisterService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
