using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocTask.Core.Dtos.UploadFile;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;

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

        public async Task<UploadFileDto> UploadFileAsync(UploadFileRequest request, int? userId)
        {
            if (userId == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} does not exist.");
            }

            if (request.File == null || request.File.Length == 0)
            {
                throw new ArgumentException("No file provided");
            }

            const long maxFileSize = 3 * 1024 * 1024; // 3 MB
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

        public async Task<UploadFileDto?> GetFileByIdAsync(int fileId)
        {
            var file = await _uploadFileRepository.GetByIdAsync(fileId);
            if (file == null)
            {
                throw new KeyNotFoundException($"File with ID {fileId} does not exist.");
            }

            var fileInfo = new FileInfo(file.FilePath);

            return new UploadFileDto
            {
                FileId = file.FileId,
                FileName = file.FileName,
                FilePath = file.FilePath,
                UploadedBy = file.UploadedBy,
                UploadedAt = file.UploadedAt,
                FileSize = fileInfo.Exists ? fileInfo.Length : 0,
                ContentType = GetContentType(file.FileName)
            };
        }

        public async Task<List<UploadFileDto>> GetFileByUserIdAsync(int userId)
        {
            var files = await _uploadFileRepository.GetByUserAsync(userId);
            if (files == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} does not exist.");
            }

            return files.Select(f =>
            {
                var fileInfo = new FileInfo(f.FilePath);

                return new UploadFileDto
                {
                    FileId = f.FileId,
                    FileName = f.FileName,
                    FilePath = f.FilePath,
                    UploadedBy = f.UploadedBy,
                    UploadedAt = f.UploadedAt,
                    FileSize = fileInfo.Exists ? fileInfo.Length : 0,
                    ContentType = GetContentType(f.FileName)
                };
            }).ToList();
        }

        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }

        public async Task<byte[]?> DownloadFileAsync(int fileId)
        {
            var file = await _uploadFileRepository.GetByIdAsync(fileId);
            if (file == null || !File.Exists(file.FilePath))
            {
                return null;
            }

            return await File.ReadAllBytesAsync(file.FilePath);             // !!!!!!!!!!!!!!!!!!!!!
        }

        public async Task<bool> DeleteFileAsync(int fileId)
        {
            var file = await _uploadFileRepository.GetByIdAsync(fileId);
            if (file == null)
            {
                throw new KeyNotFoundException($"File with ID {fileId} does not exist.");
            }

            if (File.Exists(file.FilePath))
            {
                File.Delete(file.FilePath);
            }

            return await _uploadFileRepository.DeleteAsync(fileId);
        }
    }
}