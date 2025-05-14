using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YearlyBudgetsController : ControllerBase
    {
        private readonly YearlyBudgetDB yearlyDB = new YearlyBudgetDB();

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Budget budget)
        {
            if (budget.Total != id) return BadRequest("Mismatched yearly budget ID.");
            await yearlyDB.UpdateBudget(budget);
            return NoContent();
        }
    }
}
