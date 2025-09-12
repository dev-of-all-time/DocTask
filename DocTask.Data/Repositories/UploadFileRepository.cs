using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Data.Repositories
{
    public class UploadFileRepository : IUploadFileRepository
    {
        private readonly ApplicationDbContext _context;

        public UploadFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Uploadfile> CreateAsync(Uploadfile uploadfile, int? userId)
        {
            await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            _context.Uploadfiles.Add(uploadfile);
            await _context.SaveChangesAsync();
            return uploadfile;
        }

        public async Task<bool> DeleteAsync(int fileId)
        {
            var file = await _context.Uploadfiles.FindAsync(fileId);
            if (file == null)
            {
                return false;
            }

            _context.Uploadfiles.Remove(file);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Uploadfile?> GetByIdAsync(int fileId) // Lấy theo FileId trả về file và thông tin user
        {
            return await _context.Uploadfiles
                .Include(u => u.UploadedByNavigation)
                .FirstOrDefaultAsync(u => u.FileId == fileId);
        }

        public async Task<List<Uploadfile>> GetByUserAsync(int userId) // Lấy theo UserId trả về file
        {
            return await _context.Uploadfiles
                .Where(u => u.UploadedBy == userId)
                .OrderByDescending(u => u.UploadedAt)
                .ToListAsync();
        }
    }
}