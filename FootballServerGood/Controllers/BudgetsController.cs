using FootballServerGood.DataAccess;
using FootballServerGood.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServerGood.Controllers
{
    /// <summary>
    /// API controller for managing budget-related actions.
    /// Provides endpoints for retrieving, creating, updating, and deleting team budgets.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly BudgetDB budgetDB = new BudgetDB();

        /// <summary>
        /// Retrieves all budget records for a specific team.
        /// </summary>
        /// <param name="teamId">The ID of the team whose budgets are requested.</param>
        /// <returns>A list of <see cref="Budget"/> records for the team.</returns>
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<List<Budget>>> GetByTeamId(int teamId)
        {
            var budget = await budgetDB.SelectByTeamId(teamId);
            if (budget == null)
                return NotFound(); // Returns 404 if no budget found
            return Ok(budget);     // Returns 200 with budget list
        }

        /// <summary>
        /// Updates an existing budget entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the budget to update.</param>
        /// <param name="budget">The updated <see cref="Budget"/> object.</param>
        /// <returns>No content on success, or an error if IDs mismatch.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] Budget budget)
        {
            if (id != budget.Id)
                return BadRequest("Mismatched budget ID."); // Returns 400 if path ID and body ID mismatch

            await budgetDB.UpdateBudget(budget);
            return NoContent(); // Returns 204 on success
        }

        /// <summary>
        /// Inserts a new budget entry.
        /// </summary>
        /// <param name="budget">The <see cref="Budget"/> object to insert.</param>
        /// <returns>201 Created with a route to get budgets for the team.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateBudget([FromBody] Budget budget)
        {
            await budgetDB.InsertBudget(budget);
            return CreatedAtAction(nameof(GetByTeamId), new { teamId = budget.TeamId }, budget);
        }

        /// <summary>
        /// Deletes a specific budget entry by ID.
        /// </summary>
        /// <param name="id">The ID of the budget to delete.</param>
        /// <returns>No content if deleted, or 404 if not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await budgetDB.SelectById(id);
            if (budget == null)
                return NotFound(); // Returns 404 if budget not found

            await budgetDB.DeleteBudget(budget);
            return NoContent(); // Returns 204 on success
        }
    }
}
