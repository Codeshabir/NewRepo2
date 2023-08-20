namespace Client.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class HouseDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("images")]
        public string Images { get; set; }

        [JsonPropertyName("rent")]
        public decimal? Rent { get; set; }

        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonIgnore]
        public IFormFile ImageData { get; set; } // Use IFormFile for image data

        // Other navigation properties...

        public string ImageInBase64 { get; set; }

        public int CategoryId { get; set; } // Foreign key to Category table

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }  // Navigation property to Category
    }


}

