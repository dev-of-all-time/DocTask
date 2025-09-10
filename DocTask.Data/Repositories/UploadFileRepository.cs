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

        public async Task<Uploadfile> CreateAsync(Uploadfile uploadfile, int userId)
        {
            await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            _context.Uploadfiles.Add(uploadfile);
            await _context.SaveChangesAsync();
            return uploadfile;
        }
    }
}