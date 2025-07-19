using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.IServices
{
    public interface IImageuploadService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
