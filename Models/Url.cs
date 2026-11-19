using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Models
{
    public class Url
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required long Id { get; set; }
        public required string ShortUrl { get; set; }
        public required string LongUrl { get; set; }

    }
}
