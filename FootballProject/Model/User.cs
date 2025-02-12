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
        private string? team;
        private bool? isAdmin;


        public string Name { get => name; set => name = value; }
        public string? Email { get => email; set => email = value; }
        public string? Username { get => username; set => username = value; }
        public string? Password { get => password; set => password = value; }
        public string? Team { get => team; set => team = value; }
        public bool? IsAdmin { get => isAdmin; set => isAdmin = value; }
    }
}

