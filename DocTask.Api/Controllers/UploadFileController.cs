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

        // POST: api/file
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request, int userId)
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
    }
}