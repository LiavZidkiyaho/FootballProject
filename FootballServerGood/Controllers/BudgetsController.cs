using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly BudgetDB budgetDB = new BudgetDB();

        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<Budget>> GetByTeamId(int teamId)
        {
            var budget = await budgetDB.SelectByTeamId(teamId);
            if (budget == null) return NotFound();
            return Ok(budget);
        }

        [HttpPut("{teamId}")]
        public async Task<IActionResult> UpdateBudget(int teamId, [FromBody] Budget budget)
        {
            if (budget.TeamId != teamId) return BadRequest("Mismatched team ID.");

            await budgetDB.UpdateBudget(budget);
            return NoContent();
        }
    }
}
