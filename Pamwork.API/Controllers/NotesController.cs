using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pamwork.API.Dtos;
using Pamwork.BAL.Abstract;
using Pamwork.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pamwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IMapper _mapper;

        public NotesController(INoteService noteService, IMapper mapper)
        {
            _noteService = noteService;
            _mapper = mapper;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<ActionResult<List<NoteDto>>> GetAsync()
        {
            var notes = await _noteService.GetAllAsync();
            if (notes == null || notes.Count == 0)
            {
                return NotFound("No notes found.");
            }

            return Ok(_mapper.Map<IList<NoteDto>>(notes));
        }

        // GET: api/notes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetByIdAsync(int id)
        {
            var note = await _noteService.GetByIdAsync(id);
            if (note == null)
            {
                return NotFound($"No note found with ID {id}.");
            }

            return Ok(_mapper.Map<NoteDto>(note));
        }

        // POST: api/notes
        [HttpPost]
        public async Task<ActionResult<NoteDto>> AddAsync([FromBody] NoteDto noteVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = _mapper.Map<Note>(noteVM);
            await _noteService.AddAsync(note);

            // Ensure that note.Id is populated after saving to the database
            if (note.Id == 0) // Adjust this based on how your ID is generated
            {
                return BadRequest("Note could not be added, ID is not generated.");
            }

            // Manually construct the URI of the newly created resource
            var uri = Url.Action(nameof(GetByIdAsync), new { id = note.Id });
            
            // Use Created() to return 201 Created status with the URI and note
            return Created(uri, _mapper.Map<NoteDto>(note));
        }

        // PUT: api/notes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] NoteDto noteVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != noteVM.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingNote = await _noteService.GetByIdAsync(id);
            if (existingNote == null)
            {
                return NotFound($"No note found with ID {id}.");
            }

            var updatedNote = _mapper.Map(noteVM, existingNote); // Map noteVM to the existing note
            await _noteService.UpdateAsync(updatedNote);

            return NoContent();
        }

        // DELETE: api/notes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveAsync(int id)
        {
            var noteToBeDeleted = await _noteService.GetByIdAsync(id);
            if (noteToBeDeleted == null)
            {
                return NotFound($"No note found with ID {id}.");
            }

            await _noteService.DeleteAsync(id);

            return NoContent();
        }
    }
}
