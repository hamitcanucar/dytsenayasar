using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dyt_ecommerce.DataAccess.Entities;
using dyt_ecommerce.Models;

namespace dyt_ecommerce.Services.Abstract
{
    public interface IContentService
    {
        Task<Content> Create(ContentModel model, Guid creatorId);
        Task<Content> Update(Guid id, ContentModel model);
        Task<bool> UpdateFileNames(Content content, Guid? image, Guid? file);
        Task<Content> Get(Guid id);
        Task<Content> GetUserContent(Guid id, Guid userId);

        Task<Content> Delete(Guid id);
        Task<ICollection<Content>> GetAllContent(int limit = 20, int offset = 0);
        Task<long> GetContentCount();
    }
}