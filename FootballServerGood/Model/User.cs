using System;

namespace FootballServerGood.Model
{
    /// <summary>
    /// Represents a user in the football management system.
    /// Inherits from <see cref="BaseEntity"/> to include a unique ID.
    /// </summary>
    public class User : BaseEntity
    {
        private string? name;
        private string? username;
        private string? password;
        private string? email;
        private Team team;
        private string? isAdmin;
        private string? role;

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string? Email { get => email; set => email = value; }

        /// <summary>
        /// Gets or sets the username for login purposes.
        /// </summary>
        public string? Username { get => username; set => username = value; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        public string? Password { get => password; set => password = value; }

        /// <summary>
        /// Gets or sets the team that the user is associated with.
        /// </summary>
        public Team Team { get => team; set => team = value; }

        /// <summary>
        /// Gets or sets whether the user has administrative privileges ("true"/"false").
        /// </summary>
        public string? IsAdmin { get => isAdmin; set => isAdmin = value; }

        /// <summary>
        /// Gets or sets the role of the user (e.g., "Scout", "Coach", "Analyst").
        /// </summary>
        public string? Role { get => role; set => role = value; }
    }
}
