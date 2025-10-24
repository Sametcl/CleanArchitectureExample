using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Services
{
    public interface IAwsS3Service
    {
        Task<string> UploadFileAsync(IFormFile file, string keyPrefix);
    }
}
