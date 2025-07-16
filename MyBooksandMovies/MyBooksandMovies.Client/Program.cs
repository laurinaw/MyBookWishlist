using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyBooksandMovies.Client.Services;




var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<IBookService,BookService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddHttpClient("OpenLibraryApi", client =>
{
    client.BaseAddress = new Uri("https://openlibrary.org");
});



await builder.Build().RunAsync();
