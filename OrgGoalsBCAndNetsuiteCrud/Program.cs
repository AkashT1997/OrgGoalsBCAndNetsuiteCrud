using Microsoft.OpenApi.Models;
using OrgGoalsBCAndNetsuiteCrud.MongoDB_Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<BCAuthService>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=MakeAuthorizationRequest}/{id?}");

app.Run();
