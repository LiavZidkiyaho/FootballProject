using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    /// <summary>
    /// API controller for retrieving statistical data about players.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly StatsDB statsDB = new StatsDB();

        /// <summary>
        /// Retrieves a list of statistics for a specific player and position.
        /// </summary>
        /// <param name="position">The player's position (e.g., "Forward", "Goalkeeper").</param>
        /// <param name="id">The player's ID.</param>
        /// <returns>A list of <see cref="Stat"/> objects, or a 404 if not found.</returns>
        [HttpGet("{position}/{id}")]
        public async Task<ActionResult<List<Stat>>> GetStatsByPosition(string position, int id)
        {
            var stats = await statsDB.SelectStatsByPosition(position, id);

            if (stats == null || stats.Count == 0)
                return NotFound();

            return Ok(stats);
        }
    }
}
