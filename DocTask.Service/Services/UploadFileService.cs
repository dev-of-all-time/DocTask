using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocTask.Core.Dtos.UploadFile;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Models;
using Microsoft.AspNetCore.Hosting;

namespace DocTask.Service.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IUploadFileRepository _uploadFileRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string _uploadPath;

        public UploadFileService(IUploadFileRepository uploadFileRepository, IWebHostEnvironment webHostEnvironment)
        {
            _uploadFileRepository = uploadFileRepository;
            _webHostEnvironment = webHostEnvironment;

            _uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadFolder");

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<UploadFileDto> UploadFileAsync(UploadFileRequest request, int userId)
        {
            if (request.File == null || request.File.Length == 0)
            {
                throw new ArgumentException("No file provided");
            }

            const long maxFileSize = 10 * 1024 * 1024; // 10 MB
            if (request.File.Length > maxFileSize)
            {
                throw new ArgumentException($"File size exceeds the maximum limit of {maxFileSize / (1024 * 1024)} MB");
            }

            List<string> validExtensions = new List<string>()
            {
                ".jpg", ".png", ".gif", // Image
                ".pdf", ".txt",         // Documents
                ".doc", ".docx",        // Word
                ".xls", ".xlsx",        // Excel
                ".ppt", ".pptx",        // Powerpoint
            };
            var fileExtension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
            if (!validExtensions.Contains(fileExtension))
            {
                throw new ArgumentException($"Extension is not valid({string.Join(',', validExtensions)})");
            }

            // string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            // string cleanFileName = Path.GetFileNameWithoutExtension(request.File.FileName);
            // string nameExtension = Path.GetExtension(request.File.FileName);
            // var uniqueFileName = $"{cleanFileName}_user{userId}_at_{timestamp}{nameExtension}";

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);
            await using FileStream stream = new FileStream(filePath, FileMode.Create);
            await request.File.CopyToAsync(stream);

            var relativePath = Path.Combine("uploads", uniqueFileName);
            var fileUrl = relativePath.Replace("\\", "/");

            var uploadFile = new Uploadfile
            {
                FileName = request.File.FileName,
                FilePath = fileUrl,
                UploadedBy = userId,
                UploadedAt = DateTime.UtcNow
            };

            var savedFile = await _uploadFileRepository.CreateAsync(uploadFile, userId);

            return new UploadFileDto
            {
                FileId = savedFile.FileId,
                FileName = savedFile.FileName,
                FilePath = savedFile.FilePath,
                UploadedBy = savedFile.UploadedBy,
                UploadedAt = savedFile.UploadedAt,
                FileSize = request.File.Length,
                ContentType = request.File.ContentType,
            };
        }
    }
}