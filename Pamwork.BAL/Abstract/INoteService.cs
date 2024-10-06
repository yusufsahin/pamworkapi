using Pamwork.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamwork.BAL.Abstract
{
    public interface INoteService
    {
        Task<IList<Note>> GetAllAsync();
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(int noteId);
        Task<Note> GetByIdAsync(int noteId);

    }
}
