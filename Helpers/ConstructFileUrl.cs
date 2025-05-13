namespace CarsAPI.Helpers
{
    public static class ConstructFileUrlHelper
    {
        public static string ConstructFileUrl(HttpRequest request, string folder, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            // Remove any duplicate path segments
            fileName = fileName.Replace($"uploads/{folder}/", "")
                             .Replace($"uploads\\{folder}\\", "")
                             .TrimStart('\\', '/');

            folder = folder.TrimStart('\\', '/');

            return $"{request.Scheme}://{request.Host}/uploads/{folder}/{fileName}";
        }
    }
}