using System.Text.Json;

namespace MyBooksandMovies.Client.Services;

public interface ILocalStorageService
{
    Task SetLocalStorageAsync<T>(string key, T value);

    Task<T?> GetLocalStorageAsync<T>(string key);

    Task RemoveItemLocalStorageAsync(string key);


    Task ClearLocalStorage();
   
}



