using FootballProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballProject.Services
{
    public interface IUser
    {
        //בשלב זה רק לדעת מה הפעולות שניתן לבצע באמצעות השרות
        public List<User> GetAllUsers();//החזרת רשימת כל המשתמשים
        public bool AddUser(User user);//הוספת משתמש חדש
        User GetUserByUsernameAndPassword(string username, string password);

    }
}
