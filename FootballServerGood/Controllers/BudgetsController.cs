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

        // GET: api/Budgets/team/5
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<List<Budget>>> GetByTeamId(int teamId)
        {
            var budget = await budgetDB.SelectByTeamId(teamId);
            if (budget == null)
                return NotFound();
            return Ok(budget);
        }

        // PUT: api/Budgets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] Budget budget)
        {
            if (id != budget.Id)
                return BadRequest("Mismatched budget ID.");

            await budgetDB.UpdateBudget(budget);
            return NoContent();
        }

        // POST: api/Budgets
        [HttpPost]
        public async Task<ActionResult> CreateBudget([FromBody] Budget budget)
        {
            await budgetDB.InsertBudget(budget);
            return CreatedAtAction(nameof(GetByTeamId), new { teamId = budget.TeamId }, budget);
        }

        // DELETE: api/Budgets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await budgetDB.SelectById(id);
            if (budget == null)
                return NotFound();

            await budgetDB.DeleteBudget(budget);
            return NoContent();
        }

    }
}
