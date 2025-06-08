using System;

namespace FootballProject.Model
{
    /// <summary>
    /// Represents a statistical metric associated with a player,
    /// such as goals, assists, tackles, or interceptions.
    /// </summary>
    public class Stat : BaseEntity
    {
        private string name;
        private int value;

        /// <summary>
        /// Gets or sets the name of the statistic.
        /// For example: "goals", "assists", "tackles_won".
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// Gets or sets the numerical value of the statistic.
        /// This is the total count or measurement for the stat.
        /// </summary>
        public int Value
        {
            get => value;
            set => this.value = value;
        }
    }
}
