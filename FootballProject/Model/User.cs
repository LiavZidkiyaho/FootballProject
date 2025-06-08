using System;

namespace FootballProject.Model
{
    /// <summary>
    /// Represents a system user, which can be an admin, manager, or other role,
    /// associated with a football team and login credentials.
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
        public string? Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string? Email
        {
            get => email;
            set => email = value;
        }

        /// <summary>
        /// Gets or sets the username used for login.
        /// </summary>
        public string? Username
        {
            get => username;
            set => username = value;
        }

        /// <summary>
        /// Gets or sets the user's password.
        /// Should be stored securely (e.g., hashed).
        /// </summary>
        public string? Password
        {
            get => password;
            set => password = value;
        }

        /// <summary>
        /// Gets or sets the team associated with the user.
        /// </summary>
        public Team Team
        {
            get => team;
            set => team = value;
        }

        /// <summary>
        /// Gets or sets the admin flag (e.g., "true"/"false").
        /// Consider using a boolean for better clarity.
        /// </summary>
        public string? IsAdmin
        {
            get => isAdmin;
            set => isAdmin = value;
        }

        /// <summary>
        /// Gets or sets the role of the user (e.g., "Coach", "Analyst").
        /// </summary>
        public string? Role
        {
            get => role;
            set => role = value;
        }
    }
}
