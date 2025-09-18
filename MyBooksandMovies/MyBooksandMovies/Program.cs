using MyBooksandMovies.Client.Pages;
using MyBooksandMovies.Client.Services;
using MyBooksandMovies.Components;

var builder = WebApplication.CreateBuilder(args);
// Tell Kestrel to listen on port 10000 (Render)
builder.WebHost.UseUrls("http://*:10000");

// Add services to the container.
builder.Services.AddScoped<IBookService,BookService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddHttpClient("OpenLibraryApi", client =>
{
    client.BaseAddress = new Uri("https://openlibrary.org/");
});
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


var app = builder.Build();

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


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MyBooksandMovies.Client._Imports).Assembly);

app.Run();
