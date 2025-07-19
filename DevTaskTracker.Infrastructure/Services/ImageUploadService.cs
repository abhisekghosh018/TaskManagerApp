using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevTaskTracker.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace DevTaskTracker.Infrastructure.Services
{
    public class ImageUploadService:IImageuploadService
    {
        private readonly IConfiguration _config;
        private readonly Cloudinary _cloudinary;

        public ImageUploadService(IConfiguration config)
        {
            _config = config;
            var setting = _config.GetSection("Cloudinary");
            
            Account account = new Account(
                setting["CloudName"],
                setting["ApiKey"],
                setting["ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }


        public async Task<string> UploadImageAsync(IFormFile file)
        {
            
            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.Name, stream),
                Folder = "member_image"
            };

            var uploadImgResult = await _cloudinary.UploadAsync(uploadParams);

            return  uploadImgResult.SecureUrl.ToString();
        }
        
    }

    
}
