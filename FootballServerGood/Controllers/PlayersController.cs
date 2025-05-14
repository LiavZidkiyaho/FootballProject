using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerDB playerDB = new PlayerDB();

        [HttpGet]
        public async Task<ActionResult<List<Player>>> GetAll()
        {
            return Ok(await playerDB.SelectAllPlayers());
        }

        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<List<Player>>> GetByTeam(int teamId)
        {
            return Ok(await playerDB.SelectPlayersByTeam(teamId));
        }

        [HttpGet("team/{teamId}/filter")]
        public async Task<ActionResult<List<Player>>> FilterByTeamName(int teamId, [FromQuery] string name)
        {
            return Ok(await playerDB.SelectTeamPlayersByFirstName(teamId, name));
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<Player>>> Filter([FromQuery] string field, [FromQuery] string value)
        {
            return Ok(await playerDB.SelectByFilter(field, value));
        }

        [HttpGet("sort")]
        public async Task<ActionResult<List<Player>>> Sort([FromQuery] string field, [FromQuery] string order)
        {
            return Ok(await playerDB.SelectAndSort(field, order));
        }
    }
}
