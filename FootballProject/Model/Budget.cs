using System;

namespace FootballProject.Model
{
    /// <summary>
    /// Represents a financial transaction or budget entry for a team.
    /// </summary>
    public class Budget : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ID of the team associated with this budget entry.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the budget transaction.
        /// Positive or negative depending on transaction type.
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the date the budget entry was recorded.
        /// </summary>
        public DateTime EnterDate { get; set; }

        /// <summary>
        /// Gets or sets the purpose of the budget transaction.
        /// Common values: "Transfer", "Wage", "Balance", "Difference".
        /// </summary>
        public string Purpose { get; set; }
    }
}
