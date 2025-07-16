using Microsoft.JSInterop;
using System.Text.Json;
namespace MyBooksandMovies.Client.Services;

public class LocalStorageService:ILocalStorageService
{
    private readonly IJSRuntime js;
    public LocalStorageService(IJSRuntime js)
    {
        this.js = js;
    }

    public async Task SetLocalStorageAsync<T> (string key, T value)
    {
        var json= JsonSerializer.Serialize (value);
        await js.InvokeVoidAsync("localStorage.setItem",key,json);
    }

    public async Task<T?> GetLocalStorageAsync<T>(string key)
    {
        var json = await js.InvokeAsync<string>("localStorage.getItem",key);
        if (string.IsNullOrWhiteSpace(json))
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (JsonException)
        {
            return default;
        }
    }


    
    public async Task RemoveItemLocalStorageAsync(string key)
    {
        await js.InvokeVoidAsync("localStorage.removeItem",key);
    }

    public async Task ClearLocalStorage()
    {
        await js.InvokeVoidAsync("localStorage.clear");
    }
}

