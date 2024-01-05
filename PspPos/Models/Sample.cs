namespace PspPos.Models;

public class Sample
{
    public int Id { init; get; }
    public string? Location { set; get; }
    public DateTime Date { set; get; } = DateTime.Now;

    public Sample() { }

    public Sample(int id, SamplePostModel postSample)
    {
        Id = id;
        Location = postSample.Location;
        Date = postSample.Date;
    }
}