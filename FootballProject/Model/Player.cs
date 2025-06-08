using System;
using System.Collections.Generic;

namespace FootballProject.Model
{
    /// <summary>
    /// Represents a football player with personal details, performance statistics, and team association.
    /// </summary>
    public class Player : BaseEntity
    {
        private string? fullName;
        private string? nationality;
        private DateTime dateOfBirth;
        private Team team;
        private int userValue;
        private int wage;
        private int height;
        private int weight;
        private string? foot;
        private string? position;
        private List<Stat>? stats;

        /// <summary>
        /// Gets or sets the full name of the player.
        /// </summary>
        public string? FullName { get => fullName; set => fullName = value; }

        /// <summary>
        /// Gets or sets the nationality of the player.
        /// </summary>
        public string? Nationality { get => nationality; set => nationality = value; }

        /// <summary>
        /// Gets or sets the date of birth of the player.
        /// </summary>
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }

        /// <summary>
        /// Gets or sets the team to which the player belongs.
        /// </summary>
        public Team Team { get => team; set => team = value; }

        /// <summary>
        /// Gets or sets the user's custom market value estimate for the player.
        /// </summary>
        public int UserValue { get => userValue; set => userValue = value; }

        /// <summary>
        /// Gets or sets the player's wage (e.g., per week or per season).
        /// </summary>
        public int Wage { get => wage; set => wage = value; }

        /// <summary>
        /// Gets or sets the height of the player in centimeters.
        /// </summary>
        public int Height { get => height; set => height = value; }

        /// <summary>
        /// Gets or sets the weight of the player in kilograms.
        /// </summary>
        public int Weight { get => weight; set => weight = value; }

        /// <summary>
        /// Gets or sets the dominant foot of the player (e.g., "Left", "Right").
        /// </summary>
        public string? Foot { get => foot; set => foot = value; }

        /// <summary>
        /// Gets or sets the position the player plays (e.g., "ST", "CB", "GK").
        /// </summary>
        public string? Position { get => position; set => position = value; }

        /// <summary>
        /// Gets or sets the list of statistics associated with the player.
        /// </summary>
        public List<Stat>? Stats { get => stats; set => stats = value; }
    }
}
