namespace CSharpApp.Application.Products.Commands
{
    public class CreateProductCommand : IRequest<CallResult<Product>>
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("images")]
        public List<string> Images { get; set; }
    }
}
