using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocTask.Core.Dtos.UploadFile;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Models;
using Microsoft.AspNetCore.Mvc;


namespace DockTask.Api.Controllers
{
    [ApiController]
    [Route("/ap1/v1/file")]
    public class UploadFileController : ControllerBase
    {
        private readonly IUploadFileService _uploadFileService;

        public UploadFileController(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }

        // POST: api/file/upload/{userId}
        [HttpPost("upload/{userId}")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request, int? userId)
        {
            try
            {
                var result = await _uploadFileService.UploadFileAsync(request, userId);
                var response = new ApiResponse<UploadFileDto>
                {
                    Success = true,
                    Data = result,
                    Message = "File uploaded successfully"
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<UploadFileDto>
                {
                    Success = false,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UploadFileDto>
                {
                    Success = false,
                    Error = $"Error uploading file: {ex.Message}"
                });
            }
        }

        // GET: api/file/get/{fileId}
        [HttpGet("get/{fileId}")]
        public async Task<IActionResult> GetFile(int fileId)
        {
            try
            {
                var file = await _uploadFileService.GetFileByIdAsync(fileId);
                if (file == null)
                {
                    return NotFound(new ApiResponse<UploadFileDto>
                    {
                        Success = false,
                        Error = "File not found"
                    });
                }

                return Ok(new ApiResponse<UploadFileDto>
                {
                    Success = true,
                    Data = file
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UploadFileDto>
                {
                    Success = false,
                    Error = $"Error retrieving file: {ex.Message}"
                });
            }
        }

        // GET: api/file/get/user/{userId}
        [HttpGet("get/user/{userId}")]
        public async Task<IActionResult> GetUserFile(int userId)
        {
            try
            {
                var files = await _uploadFileService.GetFileByUserIdAsync(userId);
                if (files == null)
                {
                    return NotFound(new ApiResponse<List<UploadFileDto>>
                    {
                        Success = false,
                        Error = "User not found"
                    });
                }

                return Ok(new ApiResponse<List<UploadFileDto>>
                {
                    Success = true,
                    Data = files
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<UploadFileDto>>
                {
                    Success = false,
                    Error = $"Error retrieving file: {ex.Message}"
                });
            }
        }

        // GET: api/file/get/download/{fileId}
        [HttpGet("get/download/{fileId}")]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            try
            {
                var file = await _uploadFileService.GetFileByIdAsync(fileId);
                if (file == null)
                {
                    return NotFound(new ApiResponse<object> 
                    { 
                        Success = false, 
                        Error = $"User with ID {fileId} does not exist."
                    });
                }

                var fileContent = await _uploadFileService.DownloadFileAsync(fileId);
                if (fileContent == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Error = "File not found"
                    });
                }

                return File(fileContent, file.ContentType, file.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Error = $"Error downloading file: {ex.Message}"
                });
            }
        }

        // DELETE: api/file/delete/{fileId}
        [HttpDelete("delete/{fileId}")]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            try
            {
                var result = await _uploadFileService.DeleteFileAsync(fileId);
                if (!result)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Error = "File not found"
                    });
                }

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "File deleted successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Error = $"Error deleting file: {ex.Message}"
                });
            }
        }
    }
}