namespace CSharpApp.Application.Products.Queries
{
    public class GetProductQuery : IRequest<CallResult<Product>>
    {
        public int Id { get; set; }

        public GetProductQuery(int id) => Id = id;
    }
}
