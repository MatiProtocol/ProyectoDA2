using Domain;

namespace VirtualParkApi.Response;

public class GetPointsHistoryResponse(List<PointsHistory> history, int currentBalance)
{
    public List<PointsHistoryItem> History { get; set; } = history.Select(h => new PointsHistoryItem(h)).ToList();
    public int TotalPointsEarned { get; set; } = history.Where(h => h.Points > 0).Sum(h => h.Points);
    public int TotalPointsSpent { get; set; } = Math.Abs(history.Where(h => h.Points < 0).Sum(h => h.Points));
    public int CurrentBalance { get; set; } = currentBalance;
}

public class PointsHistoryItem(PointsHistory history)
{
    public int Id { get; set; } = history.Id;
    public DateTime TransactionDate { get; set; } = history.TransactionDate;
    public int Points { get; set; } = history.Points;
    public string Origin { get; set; } = history.Origin;
    public string Description { get; set; } = history.Description;
    public string ActiveStrategy { get; set; } = history.ActiveStrategy;
}
