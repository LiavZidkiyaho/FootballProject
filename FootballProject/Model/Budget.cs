using FootballProject.Model;

public class Budget : BaseEntity
{
    // From Budget table
    public int Id { get; set; }
    public int TeamId { get; set; }
    public long Wage { get; set; }
    public int Total { get; set; }
    public int ProfitLose { get; set; }
    public long Transfer { get; set; }

    // Foreign keys (needed for JOINs)
    public int YearId { get; set; }
    public int SeasonId { get; set; }

    // From YearlyBudget table (fields named [1] to [5] in Access)
    public int One { get; set; }
    public int Two { get; set; }
    public int Three { get; set; }
    public int Four { get; set; }
    public int Five { get; set; }

    // From SeasonBudget table (monthly data)
    public int Jan { get; set; }
    public int Feb { get; set; }
    public int Mar { get; set; }
    public int Apr { get; set; }
    public int June { get; set; }
    public int July { get; set; }
    public int Aug { get; set; }
    public int Sep { get; set; }
    public int Oct { get; set; }
    public int Nov { get; set; }
    public int Dec { get; set; }
}
