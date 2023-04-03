using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLoginApi.Data;
using MyLoginApi.Models;

namespace MyLoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoemController : ControllerBase
    {
        private readonly UserContext context;

        public PoemController(UserContext context)
        {
            this.context = context;
        }

        [HttpGet("get-peoms")]
        public async Task<IEnumerable<Poem>> GetPoem() => await context.Poems.ToListAsync();
        [HttpPost("post-poems")]
        public async Task<IActionResult> Poem([FromBody] Poem poem)
        {
            if (context.Poems.Any(u => u.Id == poem.Id))
            {
                return BadRequest("Poem Already Exists");
            }
            
            var poems = new Poem
            {
                PoemTitle = poem.PoemTitle,
                PoemDescription = poem.PoemDescription,                
            };
            
            
            context.Poems.Add(poem);
            await context.SaveChangesAsync();
            return Ok(poem);
        }

        [HttpDelete("poem-delete")]
        public async Task<IActionResult> Poem(int Id)
        {
            var user = await context.Poems.FirstOrDefaultAsync(o => o.Id == Id);
            if (user == null) 
            {
                return BadRequest("Poem Doesn't exist");
            }
            context.Poems.Remove(user);
            context.SaveChanges();
            return Ok(user);
        }
    }
}
