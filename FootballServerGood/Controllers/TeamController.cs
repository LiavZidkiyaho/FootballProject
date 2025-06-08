using FootballServerGood.Model;
using FootballServerGood.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    /// <summary>
    /// API controller for managing football teams.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamDB teamDB = new TeamDB();

        /// <summary>
        /// Retrieves all teams from the database.
        /// </summary>
        /// <returns>A list of all <see cref="Team"/> objects.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetAll()
        {
            var teams = await teamDB.SelectAllTeams();
            return Ok(teams);
        }

        /// <summary>
        /// Retrieves a specific team by its ID.
        /// </summary>
        /// <param name="id">The ID of the team to retrieve.</param>
        /// <returns>The <see cref="Team"/> object if found, otherwise 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetById(int id)
        {
            var team = await teamDB.SelectById(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        /// <summary>
        /// Adds a new team to the database.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> object to add.</param>
        /// <returns>The added <see cref="Team"/> with updated ID.</returns>
        [HttpPost]
        public ActionResult<Team> AddTeam([FromBody] Team team)
        {
            teamDB.Insert(team);
            teamDB.SaveChanges();
            return Ok(team);
        }

        /// <summary>
        /// Updates an existing team.
        /// </summary>
        /// <param name="id">The ID of the team to update.</param>
        /// <param name="team">The updated <see cref="Team"/> object.</param>
        /// <returns>204 No Content if successful, 400 if ID mismatch.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateTeam(int id, [FromBody] Team team)
        {
            if (id != team.Id)
                return BadRequest("ID mismatch");

            teamDB.Update(team);
            teamDB.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deletes a team by ID.
        /// </summary>
        /// <param name="id">The ID of the team to delete.</param>
        /// <returns>204 No Content if deleted, 404 if not found.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteTeam(int id)
        {
            var team = teamDB.SelectById(id).Result;
            if (team == null) return NotFound();

            teamDB.Delete(team);
            teamDB.SaveChanges();
            return NoContent();
        }
    }
}
