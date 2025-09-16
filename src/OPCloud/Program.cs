using AntDesign.ProLayout;
using OPCloud.Client.Pages;
using OPCloud.Client.Services;
using OPCloud.Components;
using OPCloud.Services;
using OPCloud.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAntDesign();


builder.Services.AddScoped(sp =>
{
    var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
    if (httpContext != null)
    {
        return new HttpClient
        {
            BaseAddress = new Uri(httpContext.Request.Scheme + "://" + httpContext.Request.Host)
        };
    }
    return new HttpClient();
});

OPCloud.Program.AddClientServices(builder.Services);

builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<InstrumentBufferData>();
builder.Services.AddSingleton<MongoService>();
//builder.Services.AddScoped<SignalRService>();
builder.Services.AddScoped<SensorService>();


var app = builder.Build();

app.MapControllers();
app.MapHub<InstrumentHub>("/instrumentHub"); // 👈 ruta del hub

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(OPCloud.Client._Imports).Assembly);

app.Run();
