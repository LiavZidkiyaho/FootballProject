using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballProject.Model
{
    public class User : BaseEntity
    {
        private string? name;
        private string? username;
        private string? password;
        private string? email;
        private Team team;
        private string? isAdmin;
        private string? role;


        public string Name { get => name; set => name = value; }
        public string? Email { get => email; set => email = value; }
        public string? Username { get => username; set => username = value; }
        public string? Password { get => password; set => password = value; }
        public Team Team { get => team; set => team = value; }
        public string? IsAdmin { get => isAdmin; set => isAdmin = value; }
        public string? Role { get => role; set => role = value; }
    }
}

