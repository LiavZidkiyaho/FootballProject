using System;

namespace FootballProject.Model
{
    public class Budget : BaseEntity
    {
        private int id;
        private int teamId;
        private int wage;
        private int total;
        private int profitLose;
        private int transfer;

        public int Id { get => id; set => id = value; }
        public int TeamId { get => teamId; set => teamId = value; }
        public int Wage { get => wage; set => wage = value; }
        public int Total { get => total; set => total = value; }
        public int ProfitLose { get => profitLose; set => profitLose = value; }
        public int Transfer { get => transfer; set => transfer = value; }
    }
}
