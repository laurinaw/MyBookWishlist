using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBooksandMovies.Models;
public class Book
{
    [JsonPropertyName("key")]
    public string BookKey { get; set; }
    [JsonPropertyName("author_name")]
    public  List<string> Author { get; set; }
    [JsonPropertyName("cover_i")]
    public int? CoverId { get; set; }
  
    public string Title { get; set; } 
    


    [JsonPropertyName("first_publish_year")]
    public int? FirstPublishYear { get; set; }

   
    public bool IsFavorite { get; set; } = false;
    public string Description { get; set; }
    public string CoverImageUrlMedium =>
        CoverId.HasValue ? $"https://covers.openlibrary.org/b/id/{CoverId}-M.jpg" : null;

    public string CoverImageUrlLarge =>
        CoverId.HasValue ? $"https://covers.openlibrary.org/b/id/{CoverId}-L.jpg" : null;

} 


