namespace MoviesAPI.Dto;

public class PrizeInterval
{
    public string? Producer { get; set; }
    public int Interval { get; set; }
    public int PreviousWin { get; set; }
    public int FollowingWin { get; set; }
}
