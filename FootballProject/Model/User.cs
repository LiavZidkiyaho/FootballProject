using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballProject.Model
{
    public class User
    {
        private int id;
        private string? name;
        private string? username;
        private string? password;
        private string? email;


        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string? Email { get => email; set => email = value; }
        public string? Username { get => username; set => username = value; }
        public string? Password { get => password; set => password = value; }


    }
}

