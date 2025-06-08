using System;

namespace FootballServerGood.Model
{
    /// <summary>
    /// Represents a financial budget entry for a specific football team.
    /// Inherits from <see cref="BaseEntity"/> and maps to the Budget table.
    /// </summary>
    public class Budget : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the budget record.
        /// Overrides the base <see cref="BaseEntity.Id"/> property.
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the team associated with this budget entry.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the total monetary value of this budget entry.
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the date when the budget entry was recorded.
        /// </summary>
        public DateTime EnterDate { get; set; }

        /// <summary>
        /// Gets or sets the purpose of the budget entry (e.g., "Transfer", "Wage").
        /// </summary>
        public string Purpose { get; set; }
    }
}
