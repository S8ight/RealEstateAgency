using System.Text;
using Microsoft.AspNetCore.Http;

namespace REA.AuthorizationSystem.BLL.Helpers;

public static class FileConverter
{
    public static async Task<byte[]> FileToBytes(IFormFile file)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            
            if (!IsAllowedFileType(fileBytes))
            {
                throw new InvalidOperationException("Invalid file type for photo! Only .jpeg images is acceptable");
            }

            return fileBytes;
        }
    }

    private static bool IsAllowedFileType(byte[] fileBytes)
    {
        if (fileBytes.Length < 4)
        {
            return false;
        }
        
        return fileBytes[0] == 0xFF && fileBytes[1] == 0xD8;
    }
    
    
}