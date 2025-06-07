using FootballServerGood.Model;
using FootballServerGood.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamDB teamDB = new TeamDB();

        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetAll()
        {
            var teams = await teamDB.SelectAllTeams();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetById(int id)
        {
            var team = await teamDB.SelectById(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public ActionResult<Team> AddTeam([FromBody] Team team)
        {
            teamDB.Insert(team);         // or _context.Teams.Add(team);
            teamDB.SaveChanges();        // after this, team.Id should be updated

            return Ok(team);             // <== return full team with new Id
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeam(int id, [FromBody] Team team)
        {
            if (id != team.Id)
                return BadRequest("ID mismatch");

            teamDB.Update(team);
            teamDB.SaveChanges();
            return NoContent();
        }

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
