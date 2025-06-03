
using FootballProject.Model;

namespace FootballProject.Model
{
    public class Budget : BaseEntity
    {
        // From Budget table
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int Total { get; set; }
        public DateTime EnterDate { get; set; }
        public string Purpose { get; set; }
    }
}
