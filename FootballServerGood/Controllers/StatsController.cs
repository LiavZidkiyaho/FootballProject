using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly StatsDB statsDB = new StatsDB();

        // GET: api/stats/{position}/{id}
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
