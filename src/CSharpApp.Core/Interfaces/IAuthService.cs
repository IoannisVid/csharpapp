namespace CSharpApp.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetTokenAsync();
        Task<string> RefreshTokenAsync();
    }
}
