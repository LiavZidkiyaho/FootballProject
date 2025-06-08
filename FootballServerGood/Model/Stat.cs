using System;

namespace FootballServerGood.Model
{
    /// <summary>
    /// Represents a single statistical metric for a football player.
    /// </summary>
    public class Stat : BaseEntity
    {
        private string name;
        private int value;

        /// <summary>
        /// Gets or sets the name of the statistic (e.g., "Goals", "Assists", "PassesCompleted").
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// Gets or sets the value associated with the statistic.
        /// </summary>
        public int Value
        {
            get => value;
            set => this.value = value;
        }
    }
}
