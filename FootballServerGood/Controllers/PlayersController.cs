using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    /// <summary>
    /// API controller for managing player-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerDB playerDB = new PlayerDB();

        /// <summary>
        /// Retrieves all players from the database.
        /// </summary>
        /// <returns>A list of all <see cref="Player"/> records.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Player>>> GetAll()
        {
            return Ok(await playerDB.SelectAllPlayers());
        }

        /// <summary>
        /// Retrieves all players belonging to a specific team.
        /// </summary>
        /// <param name="teamId">The ID of the team.</param>
        /// <returns>A list of <see cref="Player"/> objects for the specified team.</returns>
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<List<Player>>> GetByTeam(int teamId)
        {
            return Ok(await playerDB.SelectPlayersByTeam(teamId));
        }

        /// <summary>
        /// Filters players by first name within a specific team.
        /// </summary>
        /// <param name="teamId">The ID of the team.</param>
        /// <param name="name">The first name to filter by.</param>
        /// <returns>A list of players in the team whose names match the filter.</returns>
        [HttpGet("team/{teamId}/filter")]
        public async Task<ActionResult<List<Player>>> FilterByTeamName(int teamId, [FromQuery] string name)
        {
            return Ok(await playerDB.SelectTeamPlayersByFirstName(teamId, name));
        }

        /// <summary>
        /// Filters players globally by a specific field and value.
        /// </summary>
        /// <param name="field">The field to filter by (e.g., Position, Nationality).</param>
        /// <param name="value">The value to filter by.</param>
        /// <returns>A list of matching players.</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<List<Player>>> Filter([FromQuery] string field, [FromQuery] string value)
        {
            return Ok(await playerDB.SelectByFilter(field, value));
        }

        /// <summary>
        /// Sorts players globally by a specific field and order.
        /// </summary>
        /// <param name="field">The field to sort by (e.g., Age, Goals).</param>
        /// <param name="order">The sort order (e.g., ASC or DESC).</param>
        /// <returns>A sorted list of players.</returns>
        [HttpGet("sort")]
        public async Task<ActionResult<List<Player>>> Sort([FromQuery] string field, [FromQuery] string order)
        {
            return Ok(await playerDB.SelectAndSort(field, order));
        }
    }
}
