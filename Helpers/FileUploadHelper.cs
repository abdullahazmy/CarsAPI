namespace CarsAPI.Helpers
{
    public static class FileUploadHelper
    {
        // Make these public so they can be accessed externally
        public const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        public static readonly string BaseUploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        // Allowed file extensions for upload
        private static readonly string[] AllowedFileExtensions = [".jpg", ".jpeg", ".png", ".gif"];

        // Ensure the directory exists
        private static void EnsureFolderExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            // Validate the file
            if (file == null || file.Length == 0 || file.Length > MaxFileSize)
            {
                return null;
            }

            // Get file extension and check if it's allowed
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedFileExtensions.Contains(fileExtension))
            {
                return null;
            }

            try
            {
                var uploadsFolder = Path.Combine(BaseUploadDirectory, folder);
                EnsureFolderExists(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return $"/uploads/{folder}/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File upload failed: {ex.Message}");
                return null;
            }
        }

        public static bool DeleteFile(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                return false;
            }

            try
            {
                var relativePath = fileUrl.Replace("/", Path.DirectorySeparatorChar.ToString())
                                        .Replace("\\", Path.DirectorySeparatorChar.ToString())
                                        .TrimStart(Path.DirectorySeparatorChar);

                if (relativePath.StartsWith("uploads"))
                {
                    relativePath = relativePath.Substring("uploads".Length).TrimStart(Path.DirectorySeparatorChar);
                }

                var filePath = Path.Combine(BaseUploadDirectory, relativePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File deletion failed: {ex.Message}");
                return false;
            }
        }
    }
}