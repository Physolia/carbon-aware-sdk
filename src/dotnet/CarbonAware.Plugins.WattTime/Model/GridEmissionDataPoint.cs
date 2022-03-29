namespace CarbonAware.Plugins.WattTime.Model;

[Serializable]
public record GridEmissionDataPoint
{
    public string ba { get; set; }
    public string? datatype { get; set; }
    public int? frequency { get; set; }
    public string? market { get; set; }
    public DateTime point_time { get; set; }
    public float value { get; set; }
    public string version { get; set; }

}
