namespace CSharpApp.Core.Interfaces
{
    public interface ICategoriesService
    {
        Task<IReadOnlyCollection<Category>> GetCategories();
        Task<CallResult<Category>> GetCategoryById(int Id);
        Task<CallResult<Category>> CreateCategory(CreateCategory createCategory);
    }
}
