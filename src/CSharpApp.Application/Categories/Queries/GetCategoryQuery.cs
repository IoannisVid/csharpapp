namespace CSharpApp.Application.Categories.Queries
{
    public class GetCategoryQuery : IRequest<CallResult<Category>>
    {
        public int Id { get; set; }

        public GetCategoryQuery(int id) => Id = id;
    }
}
