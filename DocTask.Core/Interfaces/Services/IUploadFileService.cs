using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocTask.Core.Dtos.UploadFile;

namespace DocTask.Core.Interfaces.Services
{
    public interface IUploadFileService
    {
        Task<UploadFileDto> UploadFileAsync(UploadFileRequest request, int? userId);
        Task<UploadFileDto?> GetFileByIdAsync(int fileId);
        Task<List<UploadFileDto>> GetFileByUserIdAsync(int userId);
        Task<bool> DeleteFileAsync(int fileId);
        Task<byte[]?> DownloadFileAsync(int fileId);
    }
}