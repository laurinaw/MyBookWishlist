using Microsoft.AspNetCore.WebUtilities;
using MyBooksandMovies.Models;
using System.Net;

namespace MyBooksandMovies.Client.Services;

public interface IBookService
{
   


    Task<OpenLibrarySearchResult> GetSearchedBooks(string searchInput);

    Task<Book> GetBookByBookKey(string bookKey);



}
