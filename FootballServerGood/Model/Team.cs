using System;

namespace FootballServerGood.Model
{
    /// <summary>
    /// Represents a football team entity with a unique identifier and a name.
    /// </summary>
    public class Team : BaseEntity
    {
        private string? team;

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        public string? team1
        {
            get => team;
            set => team = value;
        }
    }
}
