using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatApplicationAuthen.Entities.DTO;

namespace ChatApplicationAuthen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ChatContext _context;

        public FilesController(ChatContext context)
        {
            _context = context;
        }

        // GET: api/Files
        [HttpGet("getAllFiles")]
        public async Task<ActionResult<IEnumerable<File>>> GetAllFiles([FromQuery] String convId)
        {
            return await _context.Files.Where(data => data.type == 5 && data.convId == convId).ToListAsync();
        }
        [HttpGet("getAllImages")]
        public async Task<ActionResult<IEnumerable<File>>> GetAllImages([FromQuery] String convId)
        {
            return await _context.Files.Where(data => data.type == 2 && data.convId == convId).ToListAsync();
        }
        // GET: api/Files
        [HttpGet("getImages")]
        public async Task<ActionResult<IEnumerable<File>>> GetImages([FromQuery] String convId)
        {
            return await _context.Files.Where(data => data.type == 2 && data.convId == convId).Take(6).ToListAsync();
        }
        [HttpGet("getFiles")]
        public async Task<ActionResult<IEnumerable<File>>> GetFiles([FromQuery] String convId)
        {
            return await _context.Files.Where(data => data.type == 5 && data.convId == convId).Take(3).ToListAsync();
        }
        // POST: api/Files
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<File>> PostFile(File file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFile", new { id = file.Id }, file);
        }
    }
}
