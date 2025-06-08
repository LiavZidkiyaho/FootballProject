using FootballServerGood.Model;
using FootballServerGood.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    /// <summary>
    /// API controller for managing user-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDB userDB = new UserDB();

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A list of <see cref="User"/> objects.</returns>
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await userDB.SelectAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The matching <see cref="User"/> or 404 if not found.</returns>
        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<User>> GetByUsername(string username)
        {
            var user = await userDB.SelectByUsername(username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The matching <see cref="User"/> or 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await userDB.SelectById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object to add.</param>
        /// <returns>201 Created with route to newly created user.</returns>
        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            userDB.Insert(user);
            userDB.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="id">The user ID to update.</param>
        /// <param name="user">The updated <see cref="User"/> object.</param>
        /// <returns>204 NoContent if successful, 400 BadRequest if ID mismatch.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest("ID mismatch");

            userDB.Update(user);
            userDB.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>204 NoContent if deleted, or 404 NotFound if user doesn't exist.</returns>
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
