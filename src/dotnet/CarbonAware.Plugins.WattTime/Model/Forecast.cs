namespace CarbonAware.Plugins.WattTime.Model;

[Serializable]
public record Forecast
{
    public string generated_at { get; set; }

    public List<GridEmissionDataPoint> forecast { get; set; }
}
