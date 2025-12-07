using Location_voiture_front_web.Services;
using Location_voiture_front_web.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Configure API base URL
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000";

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register services first
builder.Services.AddHttpContextAccessor();

// Register Authorization Handler
builder.Services.AddTransient<AuthorizationHeaderHandler>();

// Register HttpClient with Authorization handler
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<AuthorizationHeaderHandler>();

// Register Business Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVehiculeService, VehiculeService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IPaiementService, PaiementService>();
builder.Services.AddScoped<IEntretienService, EntretienService>();
builder.Services.AddScoped<IClientService, ClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Use Session
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

