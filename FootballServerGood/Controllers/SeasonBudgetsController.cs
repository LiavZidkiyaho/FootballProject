using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonBudgetsController : ControllerBase
    {
        private readonly SeasonBudgetDB seasonDB = new SeasonBudgetDB();

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Budget budget)
        {
            if (budget.ProfitLose != id) return BadRequest("Mismatched season budget ID.");
            await seasonDB.UpdateBudget(budget);
            return NoContent();
        }
    }
}
