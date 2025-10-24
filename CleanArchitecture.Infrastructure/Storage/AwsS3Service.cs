using Amazon.S3;
using Amazon.S3.Model;
using CleanArchitecture.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Storage
{
    public sealed class AwsS3Service : IAwsS3Service
    {
        private readonly S3Options _options;
        private readonly IAmazonS3 _s3Client;

        public AwsS3Service(IOptions<S3Options> options, IAmazonS3 s3Client)
        {
            _options = options.Value;
            _s3Client = s3Client;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string keyPrefix)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Dosya boş veya seçilmemiş.");
            }

            var uniqueFileName = $"{keyPrefix}/{Guid.NewGuid()}-{file.FileName}";

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0; 

                var request = new PutObjectRequest
                {
                    BucketName = _options.BucketName,
                    Key = uniqueFileName,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    //CannedACL = S3CannedACL.PublicRead 
                };

                await _s3Client.PutObjectAsync(request);
            }
            return $"https://{_options.BucketName}.s3.{_options.Region}.amazonaws.com/{uniqueFileName}";
        }
    }
}
