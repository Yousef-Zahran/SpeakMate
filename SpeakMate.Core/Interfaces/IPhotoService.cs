using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Interfaces
{
    public interface IPhotoService
    {
       Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);
       Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
