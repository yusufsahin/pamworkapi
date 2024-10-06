using Pamwork.BAL.Abstract;
using Pamwork.DAL.Abstract;
using Pamwork.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamwork.BAL.Concrete
{
    public class NoteManager : INoteService
    {
        private INoteDal _noteDal;
        public NoteManager(INoteDal noteDal)
        {
            _noteDal = noteDal;
        }
        public async Task AddAsync(Note note)
        {
            await _noteDal.AddAsync(note, true);
        }

        public async Task DeleteAsync(int noteId)
        {
            await _noteDal.DeleteAsync(noteId, true);
        }

        public async Task<IList<Note>> GetAllAsync()
        {
            return await _noteDal.GetAllAsync();
        }

        public async Task<Note> GetByIdAsync(int noteId)
        {
            return await _noteDal.GetByIdAsync(noteId);
        }

        public async Task UpdateAsync(Note note)
        {
            await _noteDal.UpdateAsync(note, note.Id, true);
        }

    }
}
