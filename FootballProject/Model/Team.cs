using System;

namespace FootballProject.Model
{
    /// <summary>
    /// Represents a football team or club.
    /// </summary>
    public class Team : BaseEntity
    {
        private string team;

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        public string team1 { get => team; set => team = value; }
    }
}
