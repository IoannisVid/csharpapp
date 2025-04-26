namespace CSharpApp.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<CallResult<Category>>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("image")]
        public string? Image { get; set; }
    }
}
