using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocTask.Core.Models;

namespace DocTask.Core.Interfaces.Repositories
{
    public interface IUploadFileRepository
    {
        Task<Uploadfile> CreateAsync(Uploadfile uploadfile, int userId);
    }
}