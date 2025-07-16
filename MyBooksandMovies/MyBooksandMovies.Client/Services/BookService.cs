using MyBooksandMovies.Models;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text.Json;
namespace MyBooksandMovies.Client.Services;


public class BookService : IBookService
{
    private readonly IHttpClientFactory clientFactory;
    private readonly ILocalStorageService localStorage;

    public BookService(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
    {
        this.clientFactory = clientFactory;
        this.localStorage = localStorage;
    }

    private async Task<T?> ExecuteApiCall<T>(Func<Task<T>> apiCall)
    {
        try
        {
            return await apiCall();
        }
        catch (HttpRequestException ex)
        {
            return default;
        }

    }

    public async Task<OpenLibrarySearchResult> GetSearchedBooks(string searchInput)
    {
        var client = clientFactory.CreateClient("OpenLibraryApi");
        return await ExecuteApiCall(async () =>
        {

            var queryParams = new Dictionary<string, string>
            {
                ["q"] = searchInput,
                ["limit"]="150",
                ["fields"] = "title,author_name,publisher,cover_i,first_publish_year,key"

            };
            var response = await client.GetAsync(QueryHelpers.AddQueryString("/search.json", queryParams));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadFromJsonAsync<OpenLibrarySearchResult>();

                    var filteredBookList = result?.Books.Where(b => 
                                  (!string.IsNullOrWhiteSpace(b.Title) && b.Title.Contains(searchInput, StringComparison.OrdinalIgnoreCase))||
                            (b.Author != null && b.Author.Any(a => a.Contains(searchInput, StringComparison.OrdinalIgnoreCase))))
                            .ToList();


                    return new OpenLibrarySearchResult
                    {

                        Books = filteredBookList,
                        NumFound = filteredBookList.Count()
                    };

                case HttpStatusCode.InternalServerError:
                    
                     return new OpenLibrarySearchResult 
                     {   Books = new List<Book>(), 
                         NumFound = -1
                     };
                    
                
            default: return null;
            }

        });
    }


    

   public async Task<Book> GetBookByBookKey(string bookKey)
    {
        var client = clientFactory.CreateClient("OpenLibraryApi");
        return await ExecuteApiCall(async () =>
        {


            var response = await client.GetAsync($"/search.json?q={Uri.EscapeDataString(bookKey)}");
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var result = await response.Content.ReadFromJsonAsync<OpenLibrarySearchResult>();

            var detailedBook = result?.Books
                .FirstOrDefault(b => !string.IsNullOrWhiteSpace(b.BookKey) &&
                                     b.BookKey.Contains(bookKey, StringComparison.OrdinalIgnoreCase));

            if (detailedBook == null)
                return null;


            var detailResponse = await client.GetAsync($"{bookKey}.json");

            if (detailResponse.StatusCode == HttpStatusCode.OK)
            {
                var descriptionJson = await detailResponse.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(descriptionJson);

                string description = "No description available";

                if (jsonDoc.RootElement.TryGetProperty("description", out var descriptionProp))
                {
                    if (descriptionProp.ValueKind == JsonValueKind.String)
                        description = descriptionProp.GetString();
                    else if (descriptionProp.ValueKind == JsonValueKind.Object && descriptionProp.TryGetProperty("value", out var valueProp))
                        description = valueProp.GetString();
                }

                detailedBook.Description = description;
            }
            else
            {
                detailedBook.Description = "No description available";
            }

            return detailedBook;
        });
    }
}

