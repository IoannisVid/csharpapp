using System.Text.Json;

namespace CSharpApp.Core.Common
{
    public class CallResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public static CallResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static CallResult<T> Fail(string error) => new() { Success = false, ErrorMessage = error };
        public static CallResult<T> GetErrorFromResponse(string property, string errorJson)
        {
            using var doc = JsonDocument.Parse(errorJson);
            if (doc.RootElement.TryGetProperty(property, out var errorMesage))
            {
                var message = errorMesage.GetString();
                return Fail(message);
            }
            else
            {
                return Fail("Something went wrong");
            }
        }
    }
}
