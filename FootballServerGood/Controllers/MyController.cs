using FootballServerGood.Model;
using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDB userDB = new UserDB();

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await userDB.SelectAllUsers();
            return Ok(users);
        }

        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<User>> GetByUsername(string username)
        {
            var user = await userDB.SelectByUsername(username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await userDB.SelectById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            userDB.Insert(user);
            userDB.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest("ID mismatch");

            userDB.Update(user);
            userDB.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = userDB.SelectById(id).Result;
            if (user == null) return NotFound();

            userDB.Delete(user);
            userDB.SaveChanges();
            return NoContent();
        }
    }
}
