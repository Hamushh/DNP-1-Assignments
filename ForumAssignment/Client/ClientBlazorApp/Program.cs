using ClientBlazorApp.Components;
using ClientBlazorApp.Services.ServiceControllers;
using ClientBlazorApp.Services.ServiceInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IPostService, HttpPostService>();
builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<ICommentService, HttpCommentService>();

builder.Services.AddHttpClient<IPostService, HttpPostService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5173/");  // your WebAPI port
});

builder.Services.AddHttpClient<ICommentService, HttpCommentService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5173/");
});

builder.Services.AddHttpClient<IUserService, HttpUserService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5173/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
// app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
