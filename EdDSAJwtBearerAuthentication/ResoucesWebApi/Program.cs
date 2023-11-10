using EdDSAJwtBearer;
using ResoucesWebApi.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(EdDSAJwtBearerDefaults.AuthentiactionScheme)
    .AddEdDSAJwtBearer(options =>
    {
        builder.Configuration.Bind("JWT", options);
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RolePolicies.Admin, RolePolicies.AdminPolicy());
    options.AddPolicy(RolePolicies.Accountant, RolePolicies.AccountantPolicy());
    options.AddPolicy(RolePolicies.Seller, RolePolicies.SellerPolicy());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireCors("default");

app.Run();
