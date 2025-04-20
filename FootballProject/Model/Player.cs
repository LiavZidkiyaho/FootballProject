using System;

namespace FootballProject.Model
{
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

        public string FullName { get => fullName; set => fullName = value; }
        public string Nationality { get => nationality; set => nationality = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public Team Team { get => team; set => team = value; }
        public int UserValue { get => userValue; set => userValue = value; }
        public int Wage { get => wage; set => wage = value; }
        public int Height { get => height; set => height = value; }
        public int Weight { get => weight; set => weight = value; }
        public string Foot { get => foot; set => foot = value; }
        public string Position { get => position; set => position = value; }
    }
}