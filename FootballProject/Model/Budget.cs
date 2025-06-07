
using FootballProject.Model;

namespace FootballProject.Model
{
    public class Budget : BaseEntity
    {
        // From Budget table
        public int Id { get; set; }
        public int TeamId { get; set; }
        public long Total { get; set; }
        public DateTime EnterDate { get; set; }
        public string Purpose { get; set; }
    }
}
