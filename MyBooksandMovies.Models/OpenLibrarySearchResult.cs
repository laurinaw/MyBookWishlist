using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBooksandMovies.Models;
public class OpenLibrarySearchResult
{
 [JsonPropertyName("docs")]
public List<Book> Books{ get; set; }

public int NumFound { get; set; }
}



